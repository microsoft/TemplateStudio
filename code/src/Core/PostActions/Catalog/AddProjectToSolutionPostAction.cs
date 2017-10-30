// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Gen;

namespace Microsoft.Templates.Core.PostActions.Catalog
{
    public class AddProjectToSolutionPostAction : PostAction<IReadOnlyList<ICreationPath>>
    {
        public AddProjectToSolutionPostAction(string relatedTemplate, IReadOnlyList<ICreationPath> config)
            : base(relatedTemplate, config)
        {
        }

        internal override void ExecuteInternal()
        {
            var chrono = Stopwatch.StartNew();
            foreach (var output in Config)
            {
                if (!string.IsNullOrWhiteSpace(output.Path))
                {
                    var projectPath = Path.GetFullPath(Path.Combine(GenContext.Current.ProjectPath, output.Path));
                    GenContext.ToolBox.Shell.AddProjectToSolution(projectPath);
                }
            }

            chrono.Stop();
            GenContext.Current.ProjectMetrics[ProjectMetricsEnum.AddProjectToSolution] = chrono.Elapsed.TotalSeconds;
        }
    }
}
