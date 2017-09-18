// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.ObjectModel;
using System.Linq;

using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.UI.ViewModels.NewProject;
using Microsoft.Templates.UI.ViewModels.Common;
using Microsoft.Templates.Core;
using Microsoft.Templates.UI.Resources;

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

        public static bool LoadPagesGroups(ObservableCollection<ItemsGroupViewModel<TemplateInfoViewModel>> pagesGroups, string frameworkName, ref string header)
        {
            if (!pagesGroups.Any())
            {
                var pages = GenContext.ToolBox.Repo.Get(t => t.GetTemplateType() == TemplateType.Page && t.GetFrameworkList().Contains(frameworkName)).Select(t => new TemplateInfoViewModel(t, GenComposer.GetAllDependencies(t, frameworkName)));
                var groups = pages.GroupBy(t => t.Group).Select(gr => new ItemsGroupViewModel<TemplateInfoViewModel>(gr.Key as string, gr.ToList().OrderBy(t => t.Order))).OrderBy(gr => gr.Title);
                pagesGroups.AddRange(groups);
                header = string.Format(StringRes.GroupPagesHeader_SF, pages.Count());
                return pagesGroups.Any();
            }
            return false;
        }
    }
}
