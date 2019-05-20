// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.TemplateEngine.Edge.Template;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Helpers;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Core.Resources;
using Newtonsoft.Json;

namespace Microsoft.Templates.Core
{
    public class TemplatesRepository
    {
        private const string Separator = "|";

        private const string LicensesPattern = @"\[(?<text>.*?)\]\((?<url>.*?)\)\" + Separator + "?";

        private const string Catalog = "_catalog";

        private const string All = "all";

        private static readonly string[] SupportedIconTypes = { ".jpg", ".jpeg", ".png", ".xaml", ".svg" };

        public string CurrentPlatform { get; set; }

        public string CurrentLanguage { get; set; }

        public TemplatesSynchronization Sync { get; private set; }

        public string WizardVersion { get; private set; }

        public string CurrentContentFolder { get => Sync?.CurrentContent?.Path; }

        public string TemplatesVersion { get => Sync?.CurrentContent?.Version.ToString() ?? string.Empty; }

        public bool SyncInProgress { get => TemplatesSynchronization.SyncInProgress; }

        private CancellationTokenSource _cts;

        public TemplatesRepository(TemplatesSource source, Version wizardVersion, string platform, string language)
        {
            CurrentPlatform = platform;
            CurrentLanguage = language;
            WizardVersion = wizardVersion.ToString();
            Sync = new TemplatesSynchronization(source, wizardVersion);
        }

        public async Task SynchronizeAsync(bool force = false, bool removeTemplates = false)
        {
            if (removeTemplates)
            {
                Fs.SafeDeleteDirectory(CurrentContentFolder);
            }

            _cts = new CancellationTokenSource();

            await Sync.GetNewContentAsync(_cts.Token);
            if (!_cts.Token.IsCancellationRequested)
            {
                await Sync.EnsureContentAsync(force, _cts.Token);
                if (!_cts.Token.IsCancellationRequested)
                {
                    await Sync.RefreshTemplateCacheAsync(force);
                }

                Sync.CheckForWizardUpdates();
            }
        }

        public async Task RefreshAsync(bool force = false)
        {
            await Sync.EnsureContentAsync();
            await Sync.RefreshTemplateCacheAsync(force);
        }

        public void CancelSynchronization()
        {
            _cts?.Cancel();
        }

        public IEnumerable<ITemplateInfo> GetAll()
        {
            var queryResult = CodeGen.Instance.Cache.List(true, WellKnownSearchFilters.LanguageFilter(CurrentLanguage));

            return queryResult
                        .Where(r => r.IsMatch)
                        .Where(r => r.Info.GetPlatform().Equals(CurrentPlatform, StringComparison.OrdinalIgnoreCase))
                        .Select(r => r.Info)
                        .ToList();
        }

        public IEnumerable<ITemplateInfo> Get(Func<ITemplateInfo, bool> predicate)
        {
            return GetAll()
                        .Where(predicate);
        }

        public ITemplateInfo Find(Func<ITemplateInfo, bool> predicate)
        {
            return GetAll()
                        .FirstOrDefault(predicate);
        }

        public IEnumerable<MetadataInfo> GetProjectTypes(string platform)
        {
            var projectTypes = GetSupportedProjectTypes(platform);
            return GetMetadataInfo("projectTypes").Where(m => m.Platforms.Contains(platform) && projectTypes.Contains(m.Name));
        }

        public IEnumerable<MetadataInfo> GetFrontEndFrameworks(string platform, string projectType)
        {
            var frameworks = GetSupportedFx(platform, projectType);

            var results = GetMetadataInfo("frontendframeworks")
                .Where(f => f.Platforms.Contains(platform)
                            && frameworks.Any(fx => fx.Name == f.Name && fx.Type == FrameworkTypes.FrontEnd));

            results.ToList().ForEach(meta => meta.Tags["type"] = "frontend");
            return results;
        }

        public IEnumerable<MetadataInfo> GetBackEndFrameworks(string platform, string projectType)
        {
            var frameworks = GetSupportedFx(platform, projectType);

            var results = GetMetadataInfo("backendframeworks")
                .Where(f => f.Platforms.Contains(platform)
                            && frameworks.Any(fx => fx.Name == f.Name && fx.Type == FrameworkTypes.BackEnd));

            results.ToList().ForEach(meta => meta.Tags["type"] = "backend");
            return results;
        }

        public IEnumerable<ITemplateInfo> GetTemplates(TemplateType type, string platform, string projectType, string frontEndFramework = null, string backEndFramework = null)
        {
            return Get(t => t.GetTemplateType() == type
                && t.GetPlatform().Equals(platform, StringComparison.OrdinalIgnoreCase)
                && (t.GetProjectTypeList().Contains(projectType) || t.GetProjectTypeList().Contains(All))
                && IsMatchFrontEnd(t, frontEndFramework)
                && IsMatchBackEnd(t, backEndFramework));
        }

        public TemplateInfo GetTemplateInfo(ITemplateInfo template, string platform, string projectType, string frontEndFramework = null, string backEndFramework = null)
        {
            var templateInfo = new TemplateInfo
            {
                TemplateId = template.Identity,
                Name = template.Name,
                DefaultName = template.GetDefaultName(),
                Description = template.Description,
                RichDescription = template.GetRichDescription(),
                Author = template.Author,
                Version = template.GetVersion(),
                Icon = template.GetIcon(),
                DisplayOrder = template.GetDisplayOrder(),
                IsHidden = template.GetIsHidden(),
                Group = template.GetGroup(),
                IsGroupExclusiveSelection = template.GetIsGroupExclusiveSelection(),
                GenGroup = template.GetGenGroup(),
                MultipleInstance = template.GetMultipleInstance(),
                ItemNameEditable = template.GetItemNameEditable(),
                Licenses = template.GetLicenses(),
                TemplateType = template.GetTemplateType(),
                RightClickEnabled = template.GetRightClickEnabled(),
                FeatureType = template.GetFeatureType(),
            };

            var dependencies = GetDependencies(template, platform, projectType, frontEndFramework, backEndFramework, new List<ITemplateInfo>());
            templateInfo.Dependencies = GetTemplatesInfo(dependencies, platform, projectType, frontEndFramework, backEndFramework);

            return templateInfo;
        }

        public IEnumerable<TemplateInfo> GetTemplatesInfo(TemplateType type, string platform, string projectType, string frontEndFramework = null, string backEndFramework = null)
        {
            var templates = GetTemplates(type, platform, projectType, frontEndFramework, backEndFramework);
            return GetTemplatesInfo(templates, platform, projectType, frontEndFramework, backEndFramework);
        }

        public IEnumerable<TemplateInfo> GetTemplatesInfo(IEnumerable<ITemplateInfo> templates, string platform, string projectType, string frontEndFramework = null, string backEndFramework = null)
        {
            foreach (var template in templates)
            {
                yield return GetTemplateInfo(template, platform, projectType, frontEndFramework, backEndFramework);
            }
        }

        public IEnumerable<LayoutInfo> GetLayoutTemplates(string platform, string projectType, string frontEndFramework, string backEndFramework)
        {
            var projectTemplates = GetTemplates(TemplateType.Project, platform, projectType, frontEndFramework, backEndFramework);

            foreach (var projectTemplate in projectTemplates)
            {
                var layout = projectTemplate?
                .GetLayout()
                .Where(l => l.ProjectType == null || l.ProjectType.GetMultiValue().Contains(projectType));

                if (layout != null)
                {
                    foreach (var item in layout)
                    {
                        var template = Find(t => t.GroupIdentity == item.TemplateGroupIdentity
                                                                && (t.GetProjectTypeList().Contains(projectType) || t.GetProjectTypeList().Contains(All))
                                                                && IsMatchFrontEnd(t, frontEndFramework)
                                                                && IsMatchBackEnd(t, backEndFramework)
                                                                && t.GetPlatform() == platform);

                        if (template == null)
                        {
                            LogOrAlertException(string.Format(StringRes.ErrorLayoutNotFound, item.TemplateGroupIdentity, frontEndFramework, backEndFramework, platform));
                        }
                        else
                        {
                            var templateType = template.GetTemplateType();

                            if (templateType != TemplateType.Page && templateType != TemplateType.Feature)
                            {
                                LogOrAlertException(string.Format(StringRes.ErrorLayoutType, template.Identity));
                            }
                            else
                            {
                                var templateInfo = GetTemplateInfo(template, platform, projectType, frontEndFramework, backEndFramework);
                                yield return new LayoutInfo() { Layout = item, Template = templateInfo };
                            }
                        }
                    }
                }
            }
        }

        public IEnumerable<TemplateLicense> GetAllLicences(string templateId, string platform, string projectType, string frontEndFramework, string backEndFramework)
        {
            var templates = new List<ITemplateInfo>();

            var template = Find(t => t.Identity == templateId);
            templates.Add(template);
            templates.AddRange(GetDependencies(template, platform, projectType, frontEndFramework, backEndFramework, new List<ITemplateInfo>()));
            return templates.SelectMany(s => s.GetLicenses())
                .Distinct(new TemplateLicenseEqualityComparer())
                .ToList();
        }

        private IEnumerable<string> GetSupportedProjectTypes(string platform)
        {
            return GetAll()
                .Where(t => t.GetTemplateType() == TemplateType.Project
                                && t.GetPlatform() == platform)
                .SelectMany(t => t.GetProjectTypeList())
                .Distinct();
        }

        private IEnumerable<SupportedFramework> GetSupportedFx(string platform, string projectType)
        {
            var filtered = GetAll()
                          .Where(t => t.GetTemplateType() == TemplateType.Project
                          && t.GetProjectTypeList().Contains(projectType)
                          && t.GetPlatform().Equals(platform, StringComparison.OrdinalIgnoreCase)).ToList();

            var result = new List<SupportedFramework>();
            result.AddRange(filtered.SelectMany(t => t.GetFrontEndFrameworkList()).Select(name => new SupportedFramework(name, FrameworkTypes.FrontEnd)).ToList());
            result.AddRange(filtered.SelectMany(t => t.GetBackEndFrameworkList()).Select(name => new SupportedFramework(name, FrameworkTypes.BackEnd)));
            result = result.Distinct().ToList();

            return result;
        }

        public IEnumerable<ITemplateInfo> GetDependencies(ITemplateInfo template, string platform, string projectType, string frontEndFramework, string backEndFramework, IList<ITemplateInfo> dependencyList)
        {
            var dependencies = template.GetDependencyList();

            foreach (var dependency in dependencies)
            {
                var dependencyTemplate = Find(t => t.Identity == dependency
                                                                && (t.GetProjectTypeList().Contains(projectType) || t.GetProjectTypeList().Contains(All))
                                                                && IsMatchFrontEnd(t, frontEndFramework)
                                                                && IsMatchBackEnd(t, backEndFramework)
                                                                && t.GetPlatform() == platform);

                if (dependencyTemplate == null)
                {
                    LogOrAlertException(string.Format(StringRes.ErrorDependencyNotFound, dependency, frontEndFramework, backEndFramework, platform));
                }
                else
                {
                    var templateType = dependencyTemplate?.GetTemplateType();

                    if (templateType != TemplateType.Page && templateType != TemplateType.Feature)
                    {
                        LogOrAlertException(string.Format(StringRes.ErrorDependencyType, dependencyTemplate.Identity));
                    }
                    else if (dependencyTemplate.GetMultipleInstance())
                    {
                        LogOrAlertException(string.Format(StringRes.ErrorDependencyMultipleInstance, dependencyTemplate.Identity));
                    }
                    else if (dependencyList.Any(d => d.Identity == template.Identity && d.GetDependencyList().Contains(template.Identity)))
                    {
                        LogOrAlertException(string.Format(StringRes.ErrorDependencyCircularReference, template.Identity, dependencyTemplate.Identity));
                    }
                    else
                    {
                        if (!dependencyList.Contains(dependencyTemplate))
                        {
                            dependencyList.Add(dependencyTemplate);
                        }

                        GetDependencies(dependencyTemplate, platform, projectType, frontEndFramework, backEndFramework, dependencyList);
                    }
                }
            }

            return dependencyList;
        }

        private bool IsMatchFrontEnd(ITemplateInfo info, string frontEndFramework)
        {
            return string.IsNullOrEmpty(frontEndFramework)
                    || info.GetFrontEndFrameworkList().Contains(frontEndFramework, StringComparer.OrdinalIgnoreCase)
                    || info.GetFrontEndFrameworkList().Contains(All, StringComparer.OrdinalIgnoreCase);
        }

        private bool IsMatchBackEnd(ITemplateInfo info, string backEndFramework)
        {
            return string.IsNullOrEmpty(backEndFramework)
                    || info.GetBackEndFrameworkList().Contains(backEndFramework, StringComparer.OrdinalIgnoreCase)
                    || info.GetBackEndFrameworkList().Contains(All, StringComparer.OrdinalIgnoreCase);
        }

        private IEnumerable<MetadataInfo> GetMetadataInfo(string type)
        {
            var folderName = Path.Combine(Sync?.CurrentContent.Path, Catalog);

            if (!Directory.Exists(folderName))
            {
                return Enumerable.Empty<MetadataInfo>();
            }

            var metadataFile = Path.Combine(folderName, $"{type}.json");
            var metadataFileLocalized = Path.Combine(folderName, $"{CultureInfo.CurrentUICulture.IetfLanguageTag}.{type}.json");
            var metadata = JsonConvert.DeserializeObject<List<MetadataInfo>>(File.ReadAllText(metadataFile));

            if (metadata.Any(m => m.Languages != null))
            {
                metadata.RemoveAll(m => !m.Languages.Contains(CurrentLanguage));
            }

            if (File.Exists(metadataFileLocalized))
            {
                var metadataLocalized = JsonConvert.DeserializeObject<List<MetadataLocalizedInfo>>(File.ReadAllText(metadataFileLocalized));
                metadataLocalized.ForEach(ml =>
                {
                    MetadataInfo cm = metadata.FirstOrDefault(m => m.Name == ml.Name);

                    if (cm != null)
                    {
                        cm.DisplayName = ml.DisplayName;
                        cm.Summary = ml.Summary;
                    }
                });
            }

            metadata.ForEach(m => SetMetadataDescription(m, folderName, type));
            metadata.ForEach(m => SetMetadataIcon(m, folderName, type));
            metadata.ForEach(m => m.MetadataType = type == "projectTypes" ? MetadataType.ProjectType : MetadataType.Framework);
            metadata.ForEach(m => SetLicenseTerms(m));
            metadata.ForEach(m => SetDefaultTags(m));
            return metadata.OrderBy(m => m.Order);
        }

        private void SetDefaultTags(MetadataInfo metadataInfo)
        {
            if (metadataInfo.Tags == null)
            {
                metadataInfo.Tags = new Dictionary<string, object> { { "enabled", true } };
            }
            else if (!metadataInfo.Tags.ContainsKey("enabled"))
            {
                metadataInfo.Tags.Add(new KeyValuePair<string, object>("enabled", true));
            }
        }

        private void SetLicenseTerms(MetadataInfo metadataInfo)
        {
            if (!string.IsNullOrWhiteSpace(metadataInfo.Licenses))
            {
                var result = new List<TemplateLicense>();
                var licensesMatches = Regex.Matches(metadataInfo.Licenses, LicensesPattern);

                for (int i = 0; i < licensesMatches.Count; i++)
                {
                    var m = licensesMatches[i];

                    if (m.Success)
                    {
                        result.Add(new TemplateLicense
                        {
                            Text = m.Groups["text"].Value,
                            Url = m.Groups["url"].Value,
                        });
                    }
                }

                metadataInfo.LicenseTerms = result;
            }
        }

        private static void SetMetadataDescription(MetadataInfo mInfo, string folderName, string type)
        {
            var descriptionFile = Path.Combine(folderName, type, $"{CultureInfo.CurrentUICulture.IetfLanguageTag}.{mInfo.Name}.md");
            if (!File.Exists(descriptionFile))
            {
                descriptionFile = Path.Combine(folderName, type, $"{mInfo.Name}.md");
            }

            if (File.Exists(descriptionFile))
            {
                mInfo.Description = File.ReadAllText(descriptionFile);
            }
        }

        private static void SetMetadataIcon(MetadataInfo mInfo, string folderName, string type)
        {
            var iconFile = Directory
                            .EnumerateFiles(Path.Combine(folderName, type))
                            .Where(f => SupportedIconTypes.Contains(Path.GetExtension(f)))
                            .FirstOrDefault(f => Path.GetFileNameWithoutExtension(f).Equals(mInfo.Name, StringComparison.OrdinalIgnoreCase));

            if (File.Exists(iconFile))
            {
                mInfo.Icon = iconFile;
            }
        }

        private static void LogOrAlertException(string message)
        {
#if DEBUG
            throw new GenException(message);
#else
            Core.Diagnostics.AppHealth.Current.Error.TrackAsync(message).FireAndForget();
#endif
        }
    }
}
