using Microsoft.TemplateEngine.Edge.Template;
using System;
using System.IO;

namespace Microsoft.Templates.Wizard.PostActions
{
    public class AddProjectToSolutionPostAction : PostActionBase
	{
		public AddProjectToSolutionPostAction() 
			: base("AddProjectToSolution", "This post action adds the generated projects to the solution", null)
		{
		}

		public override PostActionResult Execute(GenInfo genInfo, TemplateCreationResult generationResult, GenShell shell)
		{
			//TODO: Control overwrites! What happend if the generated content already exits.
			try
			{
				//TODO: Control multiple primary outputs, continue on failure or abort?
				foreach (var output in generationResult.PrimaryOutputs)
				{
					if (!string.IsNullOrWhiteSpace(output.Path))
					{
						var projectPath = Path.GetFullPath(Path.Combine(shell.OutputPath, output.Path));
						shell.AddProjectToSolution(projectPath);
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
