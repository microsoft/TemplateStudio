using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core.Gen;
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
        public AddProjectToSolutionPostAction(IReadOnlyList<ICreationPath> config) : base(config)
        {
        }

        public override void Execute()
        {
            foreach (var output in _config)
            {
                if (!string.IsNullOrWhiteSpace(output.Path))
                {
                    var projectPath = Path.GetFullPath(Path.Combine(GenContext.Current.OutputPath, output.Path));
                    GenContext.ToolBox.Shell.AddProjectToSolution(projectPath);
                }
            }
        }
    }
}
