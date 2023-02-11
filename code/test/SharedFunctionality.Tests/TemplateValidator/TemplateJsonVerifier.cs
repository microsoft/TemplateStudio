// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ApiAnalysis;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Composition;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.PostActions;
using Newtonsoft.Json;

namespace TemplateValidator
{
    public static class TemplateJsonVerifier
    {
        private static readonly SimpleJsonAnalyzer Analyzer = new SimpleJsonAnalyzer();
        private static readonly string AllGood = Analyzer.MessageBuilder.AllGoodMessage;
        private static readonly string[] BoolStrings = { "true", "false" };

        // Verify the contents of the config file at the specified path
        public static async Task<VerifierResult> VerifyTemplatePathAsync(string configFilePath)
        {
            var results = new List<string>();

            if (configFilePath == null)
            {
                results.Add("Path to template.json file not provided.");
            }

            if (Path.GetFileName(configFilePath) != "template.json")
            {
                results.Add("Path does not point to a template.json file.");
            }

            // handle relative and absolute paths
            var rootedFilePath = configFilePath;

            if (configFilePath != null && !Path.IsPathRooted(configFilePath))
            {
                // Add 2 more level as compiled TemplateValidator project running in \bin\debug\ dir
                configFilePath = @"..\..\" + configFilePath;
                rootedFilePath = new FileInfo(configFilePath).FullName;
            }

            if (!File.Exists(rootedFilePath))
            {
                results.Add("Path to template.json file does not exist.");
            }

            if (!results.Any())
            {
                var fileContents = File.ReadAllText(configFilePath);

                // The analyzer compares the JSON with the POCO type. It identifies discrepancies in types, missing or extra properties, etc.
                var analyzerResults = await Analyzer.AnalyzeJsonAsync(fileContents, typeof(ValidationTemplateInfo));

                // The "other" checks are specific to what the wizard does with the config file and expectations of the content
                var otherResults = await PerformOtherTemplateContentChecksAsync(configFilePath, fileContents);

                results = new List<string>(analyzerResults);

                if (otherResults.Any())
                {
                    if (analyzerResults.First() == AllGood)
                    {
                        results = otherResults;
                    }
                    else
                    {
                        results.AddRange(otherResults);
                    }
                }
            }

            var success = results.Count == 1 && results.First() == AllGood;

            return new VerifierResult(success, results);
        }

        private static async Task<List<string>> PerformOtherTemplateContentChecksAsync(string filePath, string fileContents)
        {
            var results = new List<string>();

            try
            {
                var template = JsonConvert.DeserializeObject<ValidationTemplateInfo>(fileContents);

                // Composition templates don't need as much as Page and feature ones
                if (!filePath.Contains("_comp"))
                {
                    EnsureAdequateDescription(template, results);

                    // Composition templates don't need identities, but need unique names
                    EnsureVisualBasicTemplatesAreIdentifiedAppropriately(template, filePath, results, false);
                }
                else
                {
                    EnsureVisualBasicTemplatesAreIdentifiedAppropriately(template, filePath, results, true);
                }

                EnsureClassificationAsExpected(template, results);

                VerifyTagUsage(template, results);

                var templateRoot = filePath.Replace("\\.template.config\\template.json", string.Empty);

                EnsureValidPrimaryOutputPaths(template, results);

                EnsureAllDefinedPrimaryOutputsExist(template, templateRoot, results);

                EnsureAllDefinedGuidsAreUsed(template, templateRoot, results);

                VerifySymbols(template, results);

                VerifyLicensesAndProjPostactions(template, results);

                VerifyPostactionsPath(template, results);
            }
            catch (Exception ex)
            {
                results.Add($"Exception during template checks: {ex}");
            }

            await Task.CompletedTask;

            return results;
        }

        private static void VerifySymbols(ValidationTemplateInfo template, List<string> results)
        {
            if (template.Symbols == null)
            {
                return;
            }

            var type = typeof(GenParams);
            var paramValues = type.GetFields(BindingFlags.Static | BindingFlags.Public)
                                  .Where(f => f.IsLiteral)
                                  .Select(f => f.GetValue(null).ToString())
                                  .ToList();

            // The explicit values here are the ones that are currently in use.
            // In theory any string could be exported and used as a symbol but currently it's only these
            // If lots of templates start exporting new symbols it might be necessary to change how symbol keys are verified
            var allValidSymbolKeys = new List<string>(paramValues)
            {
                "baseclass", "setter",
                "wts.Page.Settings", "wts.Page.Settings.CodeBehind", "wts.Page.Settings.Prism", "wts.Page.Settings.VB", "wts.Page.Settings.CodeBehind.VB",
                "copyrightYear",
                "wts.safeprojectName",
                "commandclass",
                "onNavigatedToParams", "onNavigatedFromParams",
                "configtype", "configvalue",
                "pagetype",
                "canExecuteChangedMethodName",
                "ts.generation.appmodel",
            };

            foreach (var symbol in template.Symbols)
            {
                if (!allValidSymbolKeys.Contains(symbol.Key))
                {
                    results.Add($"Invalid Symbol key '{symbol.Key}' specified.");
                }
            }
        }

        private static void VerifyTagUsage(ValidationTemplateInfo template, List<string> results)
        {
            foreach (var tag in template.TagsCollection)
            {
                switch (tag.Key)
                {
                    case "language":
                        VerifyLanguageTagValue(tag, results);
                        break;
                    case "type":
                        VerifyTypeTagValue(tag, results);
                        break;
                    case "ts.type":
                        VerifyTSTypeTagValue(tag, results);
                        VerifyTSTypeFeatureMultipleInstancesRule(tag, template, results);
                        break;
                    case "ts.order":
                        VerifyTSOrderTagValue(results);
                        break;
                    case "ts.displayOrder":
                        VerifyTSDisplayOrderTagValue(tag, results);
                        break;
                    case "ts.compositionOrder":
                        VerifyTSCompositionOrderTagValue(tag, results);
                        break;
                    case "ts.frontendframework":
                    case "ts.backendframework":
                        VerifyTSFrameworkTagValue(tag, results);
                        break;
                    case "ts.projecttype":
                        VerifyTSProjecttypeTagValue(tag, results);
                        break;
                    case "ts.platform":
                        VerifyPlatformTagValue(tag, results);
                        break;
                    case "ts.version":
                        VerifyTSVersionTagValue(tag, results);
                        break;
                    case "ts.genGroup":
                        VerifyTSGengroupTagValue(tag, results);
                        break;
                    case "ts.rightClickEnabled":
                        VerifyTSRightclickenabledTagValue(tag, results);
                        break;
                    case "ts.compositionFilter":
                        VerifyTSCompositionFilterTagValue(tag, results);
                        VerifyTSCompositionFilterLogic(template, tag, results);
                        break;
                    case "ts.licenses":
                        VerifyTSLicensesTagValue(tag, results);
                        break;
                    case "ts.group":
                        VerifyTSGroupTagValue(tag, results);
                        break;
                    case "ts.multipleInstance":
                        VerifyTSMultipleinstanceTagValue(tag, results);
                        break;
                    case "ts.dependencies":
                        // This value is checked with the TemplateFolderVerifier
                        break;
                    case "ts.requirements":
                        // This value is checked with the TemplateFolderVerifier
                        break;
                    case "ts.exclusions":
                    // This value is checked with the TemplateFolderVerifier
                    case "ts.defaultInstance":
                        VerifyTSDefaultinstanceTagValue(tag, results);
                        break;
                    case "ts.isHidden":
                        VerifyTSIshiddenTagValue(tag, results);
                        break;
                    case "ts.isGroupExclusiveSelection":
                        VerifyTSIsGroupExclusiveSelectionTagValue(tag, results);
                        break;
                    case "ts.telemName":
                        VerifyTSTelemNameTagValue(tag, results);
                        break;
                    case "ts.outputToParent":
                        VerifyTSOutputToParentTagValue(tag, results);
                        break;
                    case "ts.requiredVsWorkload":
                        VerifyRequiredVsWorkloadTagValue(tag, results);
                        break;
                    case "ts.requiredSdks":
                        VerifyRequiredSdkTagValue(results);
                        break;
                    case "ts.requiredVersions":
                        VerifyRequiredVersionsTagValue(tag, results);
                        break;
                    case "ts.export.baseclass":
                        VerifyTSExportBaseclassTagValue(tag, results);
                        break;
                    case "ts.export.setter":
                        VerifyTSExportSetterTagValue(tag, results);
                        break;
                    case "ts.export.configtype":
                        VerifyTSExportConfigTypeTagValue(tag, results);
                        break;
                    case "ts.export.configvalue":
                        VerifyTSExportConfigValueTagValue(tag, results);
                        break;
                    case "ts.export.commandclass":
                        VerifyTSExportCommandClassTagValue(tag, results);
                        break;
                    case "ts.export.pagetype":
                        VerifyTSExportPageTypeTagValue(tag, results);
                        break;
                    case "ts.export.canExecuteChangedMethodName":
                        VerifyTSExportCanExecuteChangedMethodNameTagValue(tag, results);
                        break;
                    case "ts.export.onNavigatedToParams":
                        VerifyTSExportOnNavigatedToParamsTagValue(tag, results);
                        break;
                    case "ts.export.onNavigatedFromParams":
                        VerifyTSExportOnNavigatedFromParamsTagValue(tag, results);
                        break;
                    case "ts.appmodel":
                        VerifyTSAppModelTagValue(tag, results);
                        break;
                    default:
                        results.Add($"Unknown tag '{tag.Key}' specified in the file.");
                        break;
                }
            }

            if (template.TagsCollection.ContainsKey("language") && template.TagsCollection.ContainsKey("ts.frontendframework"))
            {
                VerifyFrameworksAreAppropriateForLanguage(template.TagsCollection["language"], template.TagsCollection["ts.frontendframework"], results);
            }
        }

        private static void VerifyTSOutputToParentTagValue(KeyValuePair<string, string> tag, List<string> results)
        {
            if (!BoolStrings.Contains(tag.Value))
            {
                results.Add($"Invalid value '{tag.Value}' specified in the ts.outputToParent tag.");
            }
        }

        private static void VerifyTSTelemNameTagValue(KeyValuePair<string, string> tag, List<string> results)
        {
            if (string.IsNullOrWhiteSpace(tag.Value))
            {
                results.Add("The tag ts.telemName cannot be blank if specified.");
            }
        }

        private static void VerifyPlatformTagValue(KeyValuePair<string, string> tag, List<string> results)
        {
            if (!new[] { Platforms.Uwp, Platforms.Wpf, Platforms.WinUI }.Contains(tag.Value))
            {
                results.Add($"Invalid value '{tag.Value}' specified in the platform tag.");
            }
        }

        private static void VerifyTSIshiddenTagValue(KeyValuePair<string, string> tag, List<string> results)
        {
            if (!BoolStrings.Contains(tag.Value))
            {
                results.Add($"Invalid value '{tag.Value}' specified in the ts.isHidden tag.");
            }
        }

        private static void VerifyTSIsGroupExclusiveSelectionTagValue(KeyValuePair<string, string> tag, List<string> results)
        {
            if (!BoolStrings.Contains(tag.Value))
            {
                results.Add($"Invalid value '{tag.Value}' specified in the ts.isGroupExclusiveSelection tag.");
            }
        }

        private static void VerifyTSExportBaseclassTagValue(KeyValuePair<string, string> tag, List<string> results)
        {
            if (!new[] { "Observable", "ObservableObject", "ViewModelBase", "INotifyPropertyChanged", "Screen", "PropertyChangedBase", "BindableBase", "ObservableRecipient" }.Contains(tag.Value))
            {
                results.Add($"Unexpected value '{tag.Value}' specified in the ts.export.baseclass tag.");
            }
        }

        private static void VerifyTSExportSetterTagValue(KeyValuePair<string, string> tag, List<string> results)
        {
            if (!new[] { "Set", "SetProperty" }.Contains(tag.Value))
            {
                results.Add($"Unexpected value '{tag.Value}' specified in the ts.export.setter tag.");
            }
        }

        private static void VerifyTSExportConfigTypeTagValue(KeyValuePair<string, string> tag, List<string> results)
        {
            if (!new[] { "IOptions<AppConfig>", "AppConfig" }.Contains(tag.Value))
            {
                results.Add($"Unexpected value '{tag.Value}' specified in the ts.export.configtype tag.");
            }
        }

        private static void VerifyTSExportConfigValueTagValue(KeyValuePair<string, string> tag, List<string> results)
        {
            if (!new[] { "appConfig.Value", "appConfig" }.Contains(tag.Value))
            {
                results.Add($"Unexpected value '{tag.Value}' specified in the ts.export.configvalue tag.");
            }
        }

        private static void VerifyTSExportCommandClassTagValue(KeyValuePair<string, string> tag, List<string> results)
        {
            if (!new[] { "RelayCommand", "DelegateCommand" }.Contains(tag.Value))
            {
                results.Add($"Unexpected value '{tag.Value}' specified in the ts.export.commandclass tag.");
            }
        }

        private static void VerifyTSExportPageTypeTagValue(KeyValuePair<string, string> tag, List<string> results)
        {
            if (!new[] { "Page", "UserControl" }.Contains(tag.Value))
            {
                results.Add($"Unexpected value '{tag.Value}' specified in the ts.export.pagetype tag.");
            }
        }

        private static void VerifyTSExportCanExecuteChangedMethodNameTagValue(KeyValuePair<string, string> tag, List<string> results)
        {
            if (!new[] { "OnCanExecuteChanged", "RaiseCanExecuteChanged", "NotifyCanExecuteChanged" }.Contains(tag.Value))
            {
                results.Add($"Unexpected value '{tag.Value}' specified in the ts.export.canExecuteChangedMethodName tag.");
            }
        }

        private static void VerifyTSExportOnNavigatedToParamsTagValue(KeyValuePair<string, string> tag, List<string> results)
        {
            if (!new[] { "object parameter", "NavigationContext navigationContext" }.Contains(tag.Value))
            {
                results.Add($"Unexpected value '{tag.Value}' specified in the ts.export.onNavigatedToParams tag.");
            }
        }

        private static void VerifyTSExportOnNavigatedFromParamsTagValue(KeyValuePair<string, string> tag, List<string> results)
        {
            if (!new[] { string.Empty, "NavigationContext navigationContext" }.Contains(tag.Value))
            {
                results.Add($"Unexpected value '{tag.Value}' specified in the ts.export.onNavigatedFromParams tag.");
            }
        }

        private static void VerifyTSAppModelTagValue(KeyValuePair<string, string> tag, List<string> results)
        {
            if (!new[] { "all", "Desktop", "Uwp" }.Contains(tag.Value))
            {
                results.Add($"Unexpected value '{tag.Value}' specified in the ts.appmodel tag.");
            }
        }

        private static void VerifyTSDefaultinstanceTagValue(KeyValuePair<string, string> tag, List<string> results)
        {
            if (string.IsNullOrWhiteSpace(tag.Value))
            {
                results.Add("The tag ts.defaultInstance cannot be blank if specified.");
            }
        }

        private static void VerifyTSMultipleinstanceTagValue(KeyValuePair<string, string> tag, List<string> results)
        {
            if (!BoolStrings.Contains(tag.Value))
            {
                results.Add($"Invalid value '{tag.Value}' specified in the ts.multipleInstance tag.");
            }
        }

        private static void VerifyTSGroupTagValue(KeyValuePair<string, string> tag, List<string> results)
        {
            if (!new[] { "Analytics", "BackgroundWork", "UserInteraction", "ApplicationLifecycle", "ApplicationLaunching", "ConnectedExperiences", "Identity", "Testing", "Data", "Tools", "Packaging" }.Contains(tag.Value))
            {
                results.Add($"Invalid value '{tag.Value}' specified in the ts.group tag.");
            }
        }

        private static void VerifyTSLicensesTagValue(KeyValuePair<string, string> tag, List<string> results)
        {
            // Allow for multiple pipe separated links
            var values = tag.Value.Split('|');

            foreach (var value in values)
            {
                // This is a really crude regex designed to catch basic variation from a markdown URI link
                if (!new Regex(@"^\[([\w .\-]){3,}\]\(http([\w ./?=\-:]){9,}\)$").IsMatch(value))
                {
                    results.Add($"'{value}' specified in the ts.licenses tag does not match the expected format.");
                }
            }
        }

        private static void VerifyTSCompositionFilterTagValue(KeyValuePair<string, string> tag, List<string> results)
        {
            try
            {
                CompositionQuery.Parse(tag.Value);
            }
            catch (InvalidCompositionQueryException ex)
            {
                results.Add($"Unable to parse the ts.compositionFilter value of '{tag.Value}': {ex}.");
            }
        }

        private static void VerifyTSCompositionFilterLogic(ValidationTemplateInfo template, KeyValuePair<string, string> tag, List<string> results)
        {
            // Ensure VB templates refer to VB identities
            if (template.TagsCollection["language"] == ProgrammingLanguages.VisualBasic)
            {
                // This can't catch everything but is better than nothing
                if (tag.Value.Contains("identity") && !tag.Value.Contains(".VB"))
                {
                    results.Add($" ts.compositionFilter identitiy value does not match the language. ({tag.Value}).");
                }
            }
        }

        private static void VerifyTSRightclickenabledTagValue(KeyValuePair<string, string> tag, List<string> results)
        {
            if (!BoolStrings.Contains(tag.Value))
            {
                results.Add($"Invalid value '{tag.Value}' specified in the ts.rightClickEnabled tag.");
            }
        }

        private static void VerifyTSGengroupTagValue(KeyValuePair<string, string> tag, List<string> results)
        {
            if (!int.TryParse(tag.Value, out int ignoredGetGroupResult))
            {
                results.Add($"The ts.genGroup tag must be an integer. Not '{tag.Value}'.");
            }
        }

        private static void VerifyTSVersionTagValue(KeyValuePair<string, string> tag, List<string> results)
        {
            if (!new Regex(@"^\d{1,2}.\d{1,2}.\d{1,2}$").IsMatch(tag.Value))
            {
                results.Add(
                    $"'{tag.Value}' specified in the ts.version tag does not match the expected format of 'X.Y.Z'.");
            }
        }

        private static void VerifyTSProjecttypeTagValue(KeyValuePair<string, string> tag, List<string> results)
        {
            // This is only used in the configuration of the project
            // This tag may contain a single value or multiple ones separated by the pipe character
            foreach (var projectType in tag.Value.Split('|'))
            {
                if (!new[] { "Blank", "NavView", "SplitView", "TabbedNav", "MenuBar", "all" }.Contains(projectType))
                {
                    results.Add($"Invalid value '{tag.Value}' specified in the ts.projecttype tag.");
                }
            }
        }

        private static string[] VbFrameworks { get; } = new[] { "CodeBehind", "MVVMToolkit" };

        private static string[] CsFrameworks { get; } = new[] { "CodeBehind", "Prism", "MVVMToolkit" };

        private static string[] AllFrameworks { get; } = new[] { "CodeBehind", "Prism", "MVVMToolkit" };

        private static void VerifyTSFrameworkTagValue(KeyValuePair<string, string> tag, List<string> results)
        {
            // This tag may contain a single value or multiple ones separated by the pipe character
            if (tag.Value != "all")
            {
                foreach (var frameworkValue in tag.Value.Split('|'))
                {
                    if (!AllFrameworks.Contains(frameworkValue))
                    {
                        results.Add($"Invalid value '{tag.Value}' specified in the ts.framework tag.");
                    }
                }
            }
        }

        private static void VerifyFrameworksAreAppropriateForLanguage(string language, string frameworks, List<string> results)
        {
            // This tag may contain a single value or multiple ones separated by the pipe character
            foreach (var frameworkValue in frameworks.Split('|'))
            {
                if (language == ProgrammingLanguages.CSharp)
                {
                    if (frameworkValue != "all")
                    {
                        if (!CsFrameworks.Contains(frameworkValue))
                        {
                            results.Add($"Invalid framework '{frameworkValue}' is not supported in templates for C# projects.");
                        }
                    }
                }
                else if (language == ProgrammingLanguages.VisualBasic)
                {
                    if (!VbFrameworks.Contains(frameworkValue))
                    {
                        results.Add($"Invalid framework '{frameworkValue}' is not supported in templates for VB.Net projects.");
                    }
                }
            }
        }

        private static void VerifyTSOrderTagValue(List<string> results)
        {
            results.Add($"The ts.order tag is no longer supported. Please use the ts.displayOrder or the ts.compositionOrder tag.");
        }

        private static void VerifyTSDisplayOrderTagValue(KeyValuePair<string, string> tag, List<string> results)
        {
            if (!int.TryParse(tag.Value, out int ignoredOrderResult))
            {
                results.Add($"The ts.displayOrder tag must be an integer. Not '{tag.Value}'.");
            }
        }

        private static void VerifyTSCompositionOrderTagValue(KeyValuePair<string, string> tag, List<string> results)
        {
            if (!int.TryParse(tag.Value, out int ignoredOrderResult))
            {
                results.Add($"The ts.compositionOrder tag must be an integer. Not '{tag.Value}'.");
            }
        }

        private static void VerifyTSTypeTagValue(KeyValuePair<string, string> tag, List<string> results)
        {
            if (!new[] { "composition", "page", "feature", "service", "testing" }.Contains(tag.Value))
            {
                results.Add($"Invalid value '{tag.Value}' specified in the ts.type tag.");
            }
        }

        private static void VerifyTSTypeFeatureMultipleInstancesRule(KeyValuePair<string, string> tag, ValidationTemplateInfo template, List<string> results)
        {
            if ("feature".Equals(tag.Value, StringComparison.Ordinal))
            {
                if (template.TagsCollection.Keys.Contains("ts.multipleInstance"))
                {
                    bool.TryParse(template.TagsCollection["ts.multipleInstance"], out var allowMultipleInstances);
                    if (!allowMultipleInstances)
                    {
                        if (!template.TagsCollection.Keys.Contains("ts.defaultInstance") || string.IsNullOrWhiteSpace(template.TagsCollection["ts.defaultInstance"]))
                        {
                            results.Add($"Template must define a valid value for ts.defaultInstance tag as ts.type is '{tag.Value}' and ts.multipleInstance is 'false'.");
                        }
                    }
                }
            }
        }

        private static void VerifyTypeTagValue(KeyValuePair<string, string> tag, List<string> results)
        {
            if (!new[] { "item", "project" }.Contains(tag.Value))
            {
                results.Add($"Invalid value '{tag.Value}' specified in the type tag.");
            }
        }

        private static void VerifyLanguageTagValue(KeyValuePair<string, string> tag, List<string> results)
        {
            if (!new[] { ProgrammingLanguages.CSharp, ProgrammingLanguages.VisualBasic, ProgrammingLanguages.Cpp }.Contains(tag.Value))
            {
                results.Add($"Invalid value '{tag.Value}' specified in the language tag.");
            }
        }

        private static void VerifyRequiredVsWorkloadTagValue(KeyValuePair<string, string> tag, List<string> results)
        {
            string[] allRequiredWorkloads = new[] { "Microsoft.VisualStudio.ComponentGroup.MSIX.Packaging", "Microsoft.VisualStudio.Workload.NetWeb", "Microsoft.VisualStudio.Workload.Universal" };

            foreach (var requiredWorkload in tag.Value.Split('|'))
            {
                if (!allRequiredWorkloads.Contains(requiredWorkload))
                {
                    results.Add($"Invalid value '{requiredWorkload}' specified in the ts.requiredVsWorkload tag.");
                }
            }
        }

        private static void VerifyRequiredSdkTagValue(List<string> results)
        {
            results.Add($"The ts.requiredSdks tag is no longer supported. Please use the ts.requiredVersions tag.");
        }

        private static void VerifyRequiredVersionsTagValue(KeyValuePair<string, string> tag, List<string> results)
        {
            string[] allVersions = new[] { "UAP, Version=10.0.19041.0", "dotnet, Version=3.1.7" };

            foreach (var version in tag.Value.Split('|'))
            {
                if (!allVersions.Contains(version))
                {
                    results.Add($"Invalid value '{version}' specified in the ts.requiredVersions tag.");
                }
            }
        }

        private static void EnsureAllDefinedGuidsAreUsed(ValidationTemplateInfo template, string templateRoot, List<string> results)
        {
            if (template.Guids != null)
            {
                var foundGuids = new List<string>();

                foreach (var file in new DirectoryInfo(templateRoot).GetFiles("*.*", SearchOption.AllDirectories))
                {
                    if (file.Name == "template.json")
                    {
                        continue;
                    }

                    var fileText = File.ReadAllText(file.FullName);

                    foreach (var guid in template.Guids)
                    {
                        if (fileText.Contains(guid))
                        {
                            foundGuids.Add(guid);
                        }
                    }
                }

                foreach (var templateGuid in template.Guids)
                {
                    if (!foundGuids.Contains(templateGuid))
                    {
                        results.Add($"Defined GUID '{templateGuid}' is not used.");
                    }
                }
            }
        }

        private static void VerifyLicensesAndProjPostactions(ValidationTemplateInfo template, List<string> results)
        {
            if (template.TagsCollection.ContainsKey("ts.licenses") && !string.IsNullOrEmpty(template.TagsCollection["ts.licenses"]))
            {
                if (template.PostActionInfos?.Count == 0)
                {
                    results.Add($"No postaction found for license defined on template {template.Identity}");
                }
            }
            else
            {
                if (template.PostActionInfos != null && template.PostActionInfos.Any(p => p.ActionId == "0B814718-16A3-4F7F-89F1-69C0F9170EAD"))
                {
                    results.Add($"Missing license on template {template.Identity}");
                }
            }
        }

        private static void VerifyPostactionsPath(ValidationTemplateInfo template, List<string> results)
        {
            if (template.PostActionInfos != null && template.PostActionInfos.Any(p => p.Args.Any(a => a.Key == "projectPath" && a.Value.Contains("/"))))
            {
                results.Add("Post-action projectPath should use '\\' instead of '/' to indicate the project file path");
            }
        }

        private static void EnsureValidPrimaryOutputPaths(ValidationTemplateInfo template, List<string> results)
        {
            if (template.PrimaryOutputs != null)
            {
                foreach (var primaryOutput in template.PrimaryOutputs)
                {
                    if (primaryOutput.Path.Contains("\\"))
                    {
                        results.Add($"Primary output '{primaryOutput.Path}' should use '/' instead of '\\'.");
                    }
                }
            }
        }

        private static void EnsureAllDefinedPrimaryOutputsExist(ValidationTemplateInfo template, string templateRoot, List<string> results)
        {
            if (template.PrimaryOutputs != null)
            {
                foreach (var primaryOutput in template.PrimaryOutputs)
                {
                    if (!File.Exists(Path.Combine(templateRoot, primaryOutput.Path)))
                    {
                        results.Add($"Primary output '{primaryOutput.Path}' does not exist.");
                    }
                }
            }
        }

        private static void EnsureClassificationAsExpected(ValidationTemplateInfo template, List<string> results)
        {
            if (template.Classifications.Count != 1)
            {
                results.Add("Only a single classification is exected.");
            }
            else if (template.Classifications.First() != "Universal")
            {
                results.Add("Classification of 'Universal' is exected.");
            }
        }

        private static void EnsureAdequateDescription(ValidationTemplateInfo template, List<string> results)
        {
            if (string.IsNullOrWhiteSpace(template.Description))
            {
                results.Add("Description not provided.");
            }
            else if (template.Description.Trim().Length < 15)
            {
                results.Add("Description is too short.");
            }
        }

        private static void EnsureVisualBasicTemplatesAreIdentifiedAppropriately(ValidationTemplateInfo template, string filePath, List<string> results, bool isCompositionTemplate)
        {
            var isVbTemplate = filePath.Contains("VB\\");

            if (!isCompositionTemplate && string.IsNullOrWhiteSpace(template.Identity))
            {
                results.Add("The template is missing an identity.");
            }
            else
            {
                if (isVbTemplate)
                {
                    if (isCompositionTemplate && !template.Name.EndsWith("VB", StringComparison.CurrentCulture))
                    {
                        results.Add("The name of templates for VisualBasic should end with 'VB'.");
                    }

                    if (!isCompositionTemplate && !template.Identity.EndsWith("VB", StringComparison.CurrentCulture))
                    {
                        results.Add("The identity of templates for VisualBasic should end with 'VB'.");
                    }
                }
                else
                {
                    if ((isCompositionTemplate && template.Name.EndsWith("VB", StringComparison.CurrentCulture)) || (!isCompositionTemplate && template.Identity.EndsWith("VB", StringComparison.CurrentCulture)))
                    {
                        results.Add("Only VisualBasic templates identities and names should end with 'VB'.");
                    }
                }
            }
        }
    }
}
