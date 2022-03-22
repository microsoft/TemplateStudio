// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core.Gen;

namespace Microsoft.Templates.Core.PostActions.Catalog
{
    public class AddProjectToContextPostAction : PostAction<IReadOnlyList<ICreationPath>>
    {
        private readonly Dictionary<string, string> _genParameters;
        private readonly string _destinationPath;
        private readonly string _generationPath;

        public AddProjectToContextPostAction(string relatedTemplate, IReadOnlyList<ICreationPath> config, Dictionary<string, string> genParameters, string destinationPath, string generationPath)
            : base(relatedTemplate, config)
        {
            _genParameters = genParameters;
            _destinationPath = destinationPath;
            _generationPath = generationPath;
        }

        internal override void ExecuteInternal()
        {
            var projectsToAdd = Config
                            .Where(o => !string.IsNullOrWhiteSpace(o.Path))
                            .Select(o => Path.GetFullPath(Path.Combine(_destinationPath, o.Path)))
                            .ToList();

            foreach (var project in projectsToAdd)
            {
                GenContext.Current.ProjectInfo.Projects.Add(project);
            }
        }
    }
}
