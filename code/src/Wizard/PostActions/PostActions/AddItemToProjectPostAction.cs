using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TemplateEngine.Edge.Template;

namespace Microsoft.Templates.Wizard.PostActions
{
	public class AddItemToProjectPostAction : PostActionBase
	{
		public AddItemToProjectPostAction()
			: base("AddItemToProject", "This post action adds the generated items to the project", null)		
		{
		}

		public override PostActionResult Execute(string outputPath, GenInfo genInfo, TemplateCreationResult generationResult, GenShell shell)
		{
			//TODO: Control overwrites! What happend if the generated content already exits.
			try
			{
				//TODO: Control multiple primary outputs, continue on failure or abort?
				foreach (var output in generationResult.PrimaryOutputs)
				{
					if (!string.IsNullOrWhiteSpace(output.Path))
					{
						var itemPath = Path.GetFullPath(Path.Combine(outputPath, output.Path));
						shell.AddItemToActiveProject(itemPath);
					}
				}

                return new PostActionResult()
                {
                    ResultCode = ResultCode.Success,
                    Message = PostActionResources.AddItemToProject_Success
				};
			}
			catch (Exception ex)
			{
				return new PostActionResult()
				{
					ResultCode = ResultCode.Error,
					Message = PostActionResources.AddItemToProject_Error,
					Exception = ex
				};
			}
		}
	}
}
