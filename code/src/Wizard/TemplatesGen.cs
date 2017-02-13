using Microsoft.TemplateEngine.Abstractions;
using Microsoft.TemplateEngine.Edge.Template;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Extensions;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Wizard.Dialog;
using Microsoft.Templates.Wizard.Host;
using Microsoft.Templates.Wizard.PostActions;
using Microsoft.Templates.Wizard.Resources;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace Microsoft.Templates.Wizard
{
    public class TemplatesGen
    {
        private TemplatesRepository _repository;
        public GenShell Shell { get; }

        //TODO: ERROR HANDLING
        public TemplatesGen(GenShell shell) : this(shell, new TemplatesRepository(new RemoteTemplatesLocation()))
        {
        }

        public TemplatesGen(GenShell shell, TemplatesRepository repository)
        {
            Shell = shell;
            _repository = repository;
        }

        //TODO: MOVE THIS SOMEWHERE
        public IEnumerable<GenInfo> ComposeGeneration(WizardState userSelection)
        {
            var genQueue = new List<GenInfo>();

            if (!string.IsNullOrEmpty(userSelection.ProjectType))
            {
                var projectTemplate = _repository
                                            .Find(t => t.GetTemplateType() == TemplateType.Project
                                                && t.GetProjectType() == userSelection.ProjectType
                                                && t.GetFrameworkList().Any(f => f == userSelection.Framework));

                var genProject = new GenInfo
                {
                    Name = Shell.ProjectName,
                    Template = projectTemplate
                };
                genQueue.Add(genProject);
                AddSystemParams(genProject);
                genProject.Parameters.Add(GenInfo.UsernameParameterName, Environment.UserName);

                var fxTemplate = _repository
                                        .Find(t => t.GetTemplateType() == TemplateType.Framework
                                            && t.Name.Equals($"{userSelection.Framework.ToLower()}.project", StringComparison.OrdinalIgnoreCase));
                if (fxTemplate != null)
                {
                    foreach (var export in fxTemplate.GetExports())
                    {
                        genProject.Parameters.Add(export.name, export.value);
                    }
                    var genFramework = new GenInfo
                    {
                        Name = Shell.ProjectName,
                        Template = fxTemplate
                    };
                    genQueue.Add(genFramework);
                    AddSystemParams(genFramework);
                }

                var fxProjectTemplate = _repository
                                            .Find(t => t.GetTemplateType() == TemplateType.Framework
                                                && t.Name.Equals($"{userSelection.Framework.ToLower()}.project.{userSelection.ProjectType.ToLower()}", StringComparison.OrdinalIgnoreCase));
                if (fxProjectTemplate != null)
                {
                    foreach (var export in fxProjectTemplate.GetExports())
                    {
                        genProject.Parameters.Add(export.name, export.value);
                    }
                    var genFramework = new GenInfo
                    {
                        Name = Shell.ProjectName,
                        Template = fxProjectTemplate
                    };
                    genQueue.Add(genFramework);
                    AddSystemParams(genFramework);
                }
            }

            foreach (var page in userSelection.Pages)
            {
                var pageTemplate = _repository.Find(t => t.Name == page.templateName);
                if (pageTemplate != null)
                {
                    var genPage = new GenInfo
                    {
                        Name = page.name,
                        Template = pageTemplate
                    };
                    genQueue.Add(genPage);
                    AddSystemParams(genPage);

                    var fxTemplate = _repository
                                        .Find(t => t.GetTemplateType() == TemplateType.Framework
                                            && t.Name.Equals($"{userSelection.Framework.ToLower()}.page", StringComparison.OrdinalIgnoreCase));
                    if (fxTemplate != null)
                    {
                        foreach (var export in fxTemplate.GetExports())
                        {
                            genPage.Parameters.Add(export.name, export.value);
                        }
                        var genFramework = new GenInfo
                        {
                            Name = page.name,
                            Template = fxTemplate
                        };
                        genQueue.Add(genFramework);
                        AddSystemParams(genFramework);
                    }

                    var fxPageTemplate = _repository
                                            .Find(t => t.GetTemplateType() == TemplateType.Framework
                                                && t.Name.Equals($"{userSelection.Framework.ToLower()}.page.{userSelection.ProjectType.ToLower()}", StringComparison.OrdinalIgnoreCase));
                    if (fxPageTemplate != null)
                    {
                        foreach (var export in fxPageTemplate.GetExports())
                        {
                            genPage.Parameters.Add(export.name, export.value);
                        }
                        var genFramework = new GenInfo
                        {
                            Name = page.name,
                            Template = fxPageTemplate
                        };
                        genQueue.Add(genFramework);
                        AddSystemParams(genFramework);
                    }

                    var fxPageNameTemplate = _repository
                                                .Find(t => t.GetTemplateType() == TemplateType.Framework
                                                    && t.Name.Equals($"{userSelection.Framework.ToLower()}.page.{pageTemplate.Name.ToLower()}", StringComparison.OrdinalIgnoreCase));

                    if (fxPageNameTemplate != null)
                    {
                        foreach (var export in fxPageNameTemplate.GetExports())
                        {
                            genPage.Parameters.Add(export.name, export.value);
                        }
                        var genFramework = new GenInfo
                        {
                            Name = page.name,
                            Template = fxPageNameTemplate
                        };
                        genQueue.Add(genFramework);
                        AddSystemParams(genFramework);
                    }
                }
            }

            return genQueue;
        }

        public WizardState GetUserSelection(WizardSteps selectionSteps)
        {
            CleanStatusBar();

            var host = new WizardHost(selectionSteps, _repository, Shell);
            Shell.ShowModal(host);

            if (host.Result != null)
            {
                //TODO: Review when right-click-actions available to track Project or Page completed.
                AppHealth.Current.Telemetry.TrackWizardCompletedAsync(WizardTypeEnum.NewProject).FireAndForget();

                return host.Result;
            }
            else
            {
                //TODO: Review when right-click-actions available to track Project or Page cancelled.
                AppHealth.Current.Telemetry.TrackWizardCancelledAsync(WizardTypeEnum.NewProject).FireAndForget();

                Shell.CancelWizard();
            }
            return null;
        }

        public void Generate(WizardState userSelection)
        {
            var genItems = ComposeGeneration(userSelection).ToList();

            Stopwatch chrono = Stopwatch.StartNew();

            Dictionary<string, TemplateCreationResult> genResults = new Dictionary<string, TemplateCreationResult>();

            var outputPath = Shell.OutputPath;

            foreach (var genInfo in genItems)
            {
                if (genInfo.Template == null)
                {
                    continue;
                }

                var statusText = GetStatusText(genInfo);

                if (!string.IsNullOrEmpty(statusText))
                {
                    Shell.ShowStatusBarMessage(statusText);
                }

                outputPath = GetOutputPath(genInfo.Template);


                AppHealth.Current.Verbose.TrackAsync($"Generating the template {genInfo.Template.Name} to {outputPath}.").FireAndForget();

                //TODO: REVIEW ASYNC
                var result = CodeGen.Instance.Creator.InstantiateAsync(genInfo.Template, genInfo.Name, null, outputPath, genInfo.Parameters, false).Result;

                genResults.Add($"{genInfo.Template.Identity}_{genInfo.Name}", result);

                if (result.Status != CreationResultStatus.CreateSucceeded)
                {
                    //TODO: THROW EXCEPTION ?
                }

                var postActionResults = ExecutePostActions(outputPath, genInfo, result);

                chrono.Stop();
            }
            PostActionCreator.CleanUpAnchors(outputPath);

            var timeSpent = chrono.Elapsed.TotalSeconds;
            TrackTelemery(genItems, genResults, timeSpent);


        }

        private static void TrackTelemery(IEnumerable<GenInfo> genItems, Dictionary<string, TemplateCreationResult> genResults, double timeSpent)
        {
            try
            {
                var pagesAdded = genItems.Where(t => t.Template.GetTemplateType() == TemplateType.Page).Count();
                var featuresAdded = genItems.Where(t => t.Template.GetTemplateType() == TemplateType.Feature).Count();

                foreach (var genInfo in genItems)
                {
                    if (genInfo.Template == null)
                    {
                        continue;
                    }
                    string appFx = genInfo.GetFramework();
                    if (string.IsNullOrEmpty(appFx))
                    {
                        // TODO: Review error tracking
                        AppHealth.Current.Error.TrackAsync("Project framework does not found").FireAndForget();
                    }
                    if (genInfo.Template.GetTemplateType() == TemplateType.Project)
                    {
                        AppHealth.Current.Telemetry.TrackProjectGenAsync(genInfo.Template, appFx, genResults[$"{genInfo.Template.Identity}_{genInfo.Name}"], pagesAdded, featuresAdded, timeSpent).FireAndForget();
                    }
                    else
                    {
                        AppHealth.Current.Telemetry.TrackPageOrFeatureTemplateGenAsync(genInfo.Template, appFx, genResults[genInfo.Template.Identity]).FireAndForget();
                    }
                }
            }
            catch (System.Exception ex)
            {
                AppHealth.Current.Exception.TrackAsync(ex, "Exception tracking telemetry for Template Generation.").FireAndForget();
            }
        }

        private string GetOutputPath(ITemplateInfo templateInfo)
        {
            if (templateInfo.GetTemplateType() == TemplateType.Project)
            {
                return Shell.OutputPath;
            }
            else
            {
                return Shell.ProjectPath;
            }
        }

        //TODO: REVIEW THIS NAME
        private void AddSystemParams(GenInfo genInfo)
        {
            genInfo.Parameters.Add("RootNamespace", Shell.GetActiveNamespace());
            genInfo.Parameters.Add("ItemNamespace", Shell.GetActiveNamespace());
        }

        private IEnumerable<PostActionResult> ExecutePostActions(string outputPath, GenInfo genInfo, TemplateCreationResult generationResult)
        {
            //Get post actions from template
            var postActions = PostActionCreator.GetPostActions(genInfo.Template);

            //Execute post action
            var postActionResults = new List<PostActionResult>();

            foreach (var postAction in postActions)
            {
                var postActionResult = postAction.Execute(outputPath, genInfo, generationResult, Shell);
                postActionResults.Add(postActionResult);
            }

            return postActionResults;
        }

        private static void ShowPostActionResults(IEnumerable<PostActionResult> postActionResults)
        {
            //TODO: Determine where to show postActionResults

            var postActionResultMessages = postActionResults.Aggregate(new StringBuilder(), (sb, a) => sb.AppendLine($"{a.Message}"), sb => sb.ToString());

            if (postActionResults.Any(p => p.ResultCode != ResultCode.Success))
            {
                var errorMessages = postActionResults
                                            .Where(p => p.ResultCode != ResultCode.Success)
                                            .Aggregate(new StringBuilder(), (sb, p) => sb.AppendLine($"{p.Message}"), sb => sb.ToString());

                //TODO: REVIEW THIS
                ErrorMessageDialog.Show("Some PostActions failed", "Failed post actions", errorMessages, MessageBoxImage.Error);
            }
        }

        private static string GetStatusText(GenInfo genInfo)
        {
            switch (genInfo.Template.GetTemplateType())
            {
                case TemplateType.Project:
                    return string.Format(StringRes.AddProjectMessage, genInfo.Name);
                case TemplateType.Page:
                    return string.Format(StringRes.AddPageMessage, $"{genInfo.Name} ({genInfo.Template.Name})");
                default:
                    return null;
            }
        }

        private void CleanStatusBar()
        {
            Shell.ShowStatusBarMessage(string.Empty);
        }
    }
}
