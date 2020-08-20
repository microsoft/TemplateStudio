// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.UI.ViewModels.Common;
using Microsoft.Templates.UI.VisualStudio;

namespace Microsoft.Templates.UI.Services
{
    public static class DataService
    {
        public static bool LoadProjectTypes(ObservableCollection<ProjectTypeMetaDataViewModel> projectTypes, string platform)
        {
            var newProjectTypes = GenContext.ToolBox.Repo.GetProjectTypes(platform)
                                    .Where(m => !string.IsNullOrEmpty(m.Description));

            var data = newProjectTypes
                        .Select(m =>
                        {
                            var targetFrameworks = GenContext.ToolBox.Repo.GetFrontEndFrameworks(platform, m.Name)
                                        .Select(fx => new FrameworkMetaDataViewModel(fx, platform))
                                        .OrderBy(f => f.Order)
                                        .ToList();

                            return new ProjectTypeMetaDataViewModel(m, platform, targetFrameworks);
                        })
                        .OrderBy(pt => pt.Order).ToList();

            projectTypes.Clear();

            if (data.Any())
            {
                foreach (var projectType in data)
                {
                    projectTypes.Add(projectType);
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool LoadFrameworks(ObservableCollection<FrameworkMetaDataViewModel> frameworks, string projectTypeName, string platform)
        {
            var targetFrameworks = GenContext.ToolBox.Repo.GetFrontEndFrameworks(platform, projectTypeName)
                                        .Select(m => new FrameworkMetaDataViewModel(m, platform))
                                        .OrderBy(f => f.Order)
                                        .ToList();

            frameworks.Clear();

            foreach (var item in targetFrameworks)
            {
                frameworks.Add(item);
            }

            return frameworks.Any();
        }

        public static bool HasTemplatesFromType(TemplateType templateType, string platform, string projectType, string frameworkName)
        {
            try
            {
                return GenContext.ToolBox.Repo.GetTemplatesInfo(templateType, platform, projectType, frameworkName)
                    .Where(t => !t.IsHidden)
                    .Any();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public static int LoadTemplatesGroups(ObservableCollection<TemplateGroupViewModel> templatesGroups, TemplateType templateType, string platform, string projectType, string frameworkName, bool loadFromRightClick = false)
        {
            if (!templatesGroups.Any())
            {
                var templates = GenContext.ToolBox.Repo.GetTemplatesInfo(templateType, platform, projectType, frameworkName)
                    .Where(t => !t.IsHidden);

                if (loadFromRightClick)
                {
                    templates = templates.Where(t => t.RightClickEnabled);
                }

                var templateViewModel = templates.Select(t => new TemplateInfoViewModel(t, platform, projectType, frameworkName));
                var groups = templateViewModel
                    .OrderBy(t => t.Order)
                    .GroupBy(t => t.Group)
                    .Select(gr => new TemplateGroupViewModel(gr))
                    .OrderBy(gr => gr.Name);
                templatesGroups.AddRange(groups);

                return templates.Count();
            }

            return 0;
        }

        public static bool HasAllVisualStudioWorkloads(IEnumerable<string> workloadIds)
        {
            var vsShell = GenContext.ToolBox.Shell as VsGenShell;

            // If not in VS then assume all workloads are available.
            if (vsShell != null && vsShell.GetInstalledPackageIds().Any())
            {
                var installedIds = vsShell.GetInstalledPackageIds();

                foreach (var workloadId in workloadIds)
                {
                    if (!installedIds.Contains(workloadId))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
