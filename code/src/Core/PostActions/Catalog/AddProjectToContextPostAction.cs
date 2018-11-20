// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core.Gen;

namespace Microsoft.Templates.Core.PostActions.Catalog
{
    public class AddProjectToContextPostAction : PostAction<IReadOnlyList<ICreationPath>>
    {
        private readonly Dictionary<string, string> _genParameters;
        private readonly string _destinationPath;
        private readonly string[] targetFrameworkTags = { "</TargetFramework>", "</TargetFrameworks>" };

        public AddProjectToContextPostAction(string relatedTemplate, IReadOnlyList<ICreationPath> config, Dictionary<string, string> genParameters, string destinationPath)
            : base(relatedTemplate, config)
        {
            _genParameters = genParameters;
            _destinationPath = destinationPath;
        }

        internal override async Task ExecuteInternalAsync()
        {
            var projectsToAdd = Config
                            .Where(o => !string.IsNullOrWhiteSpace(o.Path))
                            .Select(o => Path.GetFullPath(Path.Combine(_destinationPath, o.GetOutputPath(_genParameters))))
                            .ToList();

            foreach (var project in projectsToAdd)
            {
                // Detect if project is CPS project system based
                // https://github.com/dotnet/project-system/blob/master/docs/opening-with-new-project-system.md
                var isCPSProject = targetFrameworkTags.Any(t => File.ReadAllText(project).Contains(t));
                GenContext.Current.Projects.Add(new ProjectInfo { ProjectPath = project, IsCPSProject = isCPSProject });
            }

            await Task.CompletedTask;
        }
    }
}
