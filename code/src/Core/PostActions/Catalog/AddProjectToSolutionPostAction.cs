using Microsoft.TemplateEngine.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.PostActions.Catalog
{
    public class AddProjectToSolutionPostAction : PostAction<IReadOnlyList<ICreationPath>>
    {
        public AddProjectToSolutionPostAction(GenShell shell, IReadOnlyList<ICreationPath> config) : base(shell, config)
        {
        }

        public override void Execute()
        {
            foreach (var output in _config)
            {
                if (!string.IsNullOrWhiteSpace(output.Path))
                {
                    var projectPath = Path.GetFullPath(Path.Combine(_shell.OutputPath, output.Path));
                    _shell.AddProjectToSolution(projectPath);
                }
            }
        }
    }
}
