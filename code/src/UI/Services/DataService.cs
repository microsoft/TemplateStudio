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
        public static bool LoadProjectTypes(ObservableCollection<MetadataInfoViewModel> projectTypes, string platform)
        {
            var templateProjectTypes = GenComposer.GetSupportedProjectTypes(platform);

            var data = GenContext.ToolBox.Repo.GetProjectTypes(platform)
                        .Where(m => templateProjectTypes.Contains(m.Name) && !string.IsNullOrEmpty(m.Description))
                        .Select(m => new MetadataInfoViewModel(m))
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

        public static bool LoadFrameworks(ObservableCollection<MetadataInfoViewModel> frameworks, string projectTypeName, string platform)
        {
            var templateFrameworks = GenComposer.GetSupportedFx(projectTypeName, platform);

            var targetFrameworks = GenContext.ToolBox.Repo.GetFrameworks(platform)
                                        .Where(m => templateFrameworks.Contains(m.Name))
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

        public static int LoadTemplatesGroups(ObservableCollection<TemplateGroupViewModel> templatesGroups, TemplateType templateType, string frameworkName, string platform)
        {
            if (!templatesGroups.Any())
            {
                var templates = GenContext.ToolBox.Repo.Get(t =>
                                    t.GetTemplateType() == templateType &&
                                    t.GetFrameworkList().Contains(frameworkName) &&
                                    t.GetPlatform() == platform &&
                                    !t.GetIsHidden())
                                    .Select(t => new TemplateInfoViewModel(t, frameworkName, platform));

                var groups = templates
                    .OrderBy(t => t.Order)
                    .GroupBy(t => t.Group)
                    .Select(gr => new TemplateGroupViewModel(gr))
                    .OrderBy(gr => gr.Name);
                templatesGroups.AddRange(groups);
                return templates.Count();
            }

            return 0;
        }
    }
}
