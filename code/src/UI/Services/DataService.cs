// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.ObjectModel;
using System.Linq;

using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.UI.ViewModels.Common;
using Microsoft.Templates.UI.ViewModels.NewProject;

namespace Microsoft.Templates.UI.Services
{
    public static class DataService
    {
        public static bool LoadProjectTypes(ObservableCollection<MetadataInfoViewModel> projectTypes)
        {
            projectTypes.Clear();
            if (GenContext.ToolBox.Repo.GetProjectTypes().Any())
            {
                var data = GenContext.ToolBox.Repo.GetProjectTypes().Select(m => new MetadataInfoViewModel(m)).ToList();

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
                                                                .ToList();

            frameworks.Clear();

            foreach (var item in targetFrameworks)
            {
                frameworks.Add(item);
            }

            return frameworks.Any();
        }

        public static int LoadTemplatesGroups(ObservableCollection<ItemsGroupViewModel<TemplateInfoViewModel>> templatesGroups, TemplateType templateType, string frameworkName)
        {
            if (!templatesGroups.Any())
            {
                var templates = GenContext.ToolBox.Repo.Get(t => t.GetTemplateType() == templateType && t.GetFrameworkList().Contains(frameworkName) && !t.GetIsHidden()).Select(t => new TemplateInfoViewModel(t, GenComposer.GetAllDependencies(t, frameworkName)));
                var groups = templates.GroupBy(t => t.Group).Select(gr => new ItemsGroupViewModel<TemplateInfoViewModel>(gr.Key as string, gr.ToList().OrderBy(t => t.Order))).OrderBy(gr => gr.Name);
                templatesGroups.AddRange(groups);
                return templates.Count();
            }

            return 0;
        }
    }
}
