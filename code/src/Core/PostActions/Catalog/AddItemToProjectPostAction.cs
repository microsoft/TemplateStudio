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
    public class AddItemToProjectPostAction : PostAction<IReadOnlyList<ICreationPath>>
    {
        public AddItemToProjectPostAction(IReadOnlyList<ICreationPath> config) : base(config)
        {
        }

        public override void Execute()
        {
            var itemsToAdd = _config
                                .Where(o => !string.IsNullOrWhiteSpace(o.Path))
                                .Select(o => Path.GetFullPath(Path.Combine(GenContext.Current.OutputPath, o.Path)))
                                .ToList();

            GenContext.ToolBox.Shell.AddItems(itemsToAdd.ToArray());
        }
    }
}
