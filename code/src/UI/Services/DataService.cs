// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.UI.ViewModels.Common;

namespace Microsoft.Templates.UI.Services
{
    public static class DataService
    {
        public static bool LoadProjectTypes(ObservableCollection<MetadataInfoViewModel> projectTypes)
        {
            projectTypes.Clear();
            if (GenContext.ToolBox.Repo.GetProjectTypes().Any())
            {
                var data = GenContext.ToolBox.Repo.GetProjectTypes()
                                                .Select(m => new MetadataInfoViewModel(m))
                                                .OrderBy(pt => pt.Order)
                                                .ToList();

                foreach (var projectType in data.Where(p => !string.IsNullOrEmpty(p.Description)))
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

        public static bool LoadFrameworks(ObservableCollection<MetadataInfoViewModel> frameworks, string projectTypeName)
        {
            var projectFrameworks = GenComposer.GetSupportedFx(projectTypeName);
            var targetFrameworks = GenContext.ToolBox.Repo.GetFrameworks()
                                                                .Where(m => projectFrameworks.Contains(m.Name))
                                                                .Select(m => new MetadataInfoViewModel(m))
                                                                .OrderBy(f => f.Order)
                                                                .ToList();

            frameworks.Clear();

            foreach (var item in targetFrameworks)
            {
                frameworks.Add(item);
            }

            return frameworks.Any();
        }

        public static int LoadTemplateGroups(ObservableCollection<TemplateGroupViewModel> templateGroups, TemplateType templateType, string frameworkName)
        {
            if (!templateGroups.Any())
            {
                var templates = GenContext.ToolBox.Repo.Get(t =>
                                    t.GetTemplateType() == templateType &&
                                    t.GetFrameworkList().Contains(frameworkName) &&
                                    !t.GetIsHidden())
                                    .Select(t => new TemplateInfoViewModel(t, frameworkName));

                var groups = templates
                    .OrderBy(t => t.Order)
                    .GroupBy(t => t.Group)
                    .Select(gr => new TemplateGroupViewModel(gr))
                    .OrderBy(gr => gr.Name);
                templateGroups.AddRange(groups);
                return templates.Count();
            }

            return 0;
        }
    }
}
