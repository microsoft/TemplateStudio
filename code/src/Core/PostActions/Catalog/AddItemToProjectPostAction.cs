using Microsoft.TemplateEngine.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.PostActions.Catalog
{
    public class AddItemToProjectPostAction : PostAction<IReadOnlyList<ICreationPath>>
    {
        public AddItemToProjectPostAction(GenShell shell, IReadOnlyList<ICreationPath> config) : base(shell, config)
        {
        }

        public override void Execute()
        {
            foreach (var output in _config)
            {
                if (!string.IsNullOrWhiteSpace(output.Path))
                {
                    //TODO: REVIEW OUTPUT PATH
                    var projectPath = Path.GetFullPath(Path.Combine(_shell.ProjectPath, output.Path));
                    _shell.AddItemToActiveProject(projectPath);
                }
            }
        }
    }
}
