// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.UI.ViewModels.Common;
using Microsoft.Templates.UI.VisualStudio.GenShell;

namespace Microsoft.Templates.UI.Services
{
    public static class DataService
    {
        public static bool LoadProjectTypes(ObservableCollection<ProjectTypeMetaDataViewModel> projectTypes, UserSelectionContext context)
        {
            var newProjectTypes = GenContext.ToolBox.Repo.GetProjectTypes(context)
                                    .Where(m => !string.IsNullOrEmpty(m.Description));

            var data = newProjectTypes
                        .Select(m =>
                        {
                            context.ProjectType = m.Name;
                            var targetFrameworks = GenContext.ToolBox.Repo.GetFrontEndFrameworks(context)
                                        .Select(fx => new FrameworkMetaDataViewModel(fx))
                                        .OrderBy(f => f.Order)
                                        .ToList();

                            return new ProjectTypeMetaDataViewModel(m, context, targetFrameworks);
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

        public static bool LoadFrameworks(ObservableCollection<FrameworkMetaDataViewModel> frameworks, UserSelectionContext context)
        {
            var targetFrameworks = GenContext.ToolBox.Repo.GetFrontEndFrameworks(context)
                                        .Select(m => new FrameworkMetaDataViewModel(m))
                                        .OrderBy(f => f.Order)
                                        .ToList();

            frameworks.Clear();

            foreach (var item in targetFrameworks)
            {
                frameworks.Add(item);
            }

            return frameworks.Any();
        }

        public static bool HasTemplatesFromType(TemplateType templateType, UserSelectionContext context)
        {
            return GenContext.ToolBox.Repo.GetTemplatesInfo(templateType, context)
                .Where(t => !t.IsHidden)
                .Any();
        }

        public static int LoadTemplatesGroups(ObservableCollection<TemplateGroupViewModel> templatesGroups, TemplateType templateType, UserSelectionContext context, bool loadFromRightClick = false)
        {
            if (!templatesGroups.Any())
            {
                var templates = GenContext.ToolBox.Repo.GetTemplatesInfo(templateType, context)
                    .Where(t => !t.IsHidden);

                if (loadFromRightClick)
                {
                    templates = templates.Where(t => t.RightClickEnabled);
                }

                var templateViewModel = templates.Select(t => new TemplateInfoViewModel(t, context));
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
            // If not in VS then assume all workloads are available.
            if (GenContext.ToolBox.Shell is VsGenShell vsShell && vsShell.VisualStudio.GetInstalledPackageIds().Any())
            {
                var installedIds = vsShell.VisualStudio.GetInstalledPackageIds();

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
