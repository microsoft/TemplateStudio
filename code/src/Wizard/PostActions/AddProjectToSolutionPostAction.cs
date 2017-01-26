using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TemplateEngine.Edge.Template;
using Microsoft.Templates.Wizard.Vs;

namespace Microsoft.Templates.Wizard.PostActions
{
	public class AddProjectToSolutionPostAction : PostActionBase
	{
		public AddProjectToSolutionPostAction() 
			: base("AddProjectToSolution", "This post action adds the generated projects to the solution", null)
		{
		}

		public override PostActionResult Execute(ExecutionContext context, TemplateCreationResult generationResult, IVisualStudioShell vsShell)
		{
			//TODO: Control overwrites! What happend if the generated content already exits.
			try
			{
				//TODO: Control multiple primary outputs, continue on failure or abort?
				foreach (var output in generationResult.PrimaryOutputs)
				{
					if (!string.IsNullOrWhiteSpace(output.Path))
					{
						var projectPath = Path.GetFullPath(Path.Combine(context.SolutionPath, output.Path));
						vsShell.AddProjectToSolution(projectPath);
					}
				}
				return new PostActionResult()
				{
					ResultCode = ResultCode.Success,
					Message = $"Postaction {Name}: Successfully added projects to solution"
				};
			}
			catch (Exception ex)
			{
				return new PostActionResult()
				{
					ResultCode = ResultCode.Error,
					Message = $"Postaction {Name}: Error adding projects to solution",
					Exception = ex
				};
			}
		}
	}
}
