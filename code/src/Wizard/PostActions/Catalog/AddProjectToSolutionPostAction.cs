using Microsoft.TemplateEngine.Edge.Template;
using Microsoft.Templates.Core;
using System;
using System.IO;

namespace Microsoft.Templates.Wizard.PostActions.Catalog
{
    public class AddProjectToSolutionPostAction : PostActionBase
	{
		public AddProjectToSolutionPostAction() 
			: base("AddProjectToSolution", "This post action adds the generated projects to the solution", null)
		{
		}

		public override PostActionResult Execute(string outputPath, GenInfo genInfo, TemplateCreationResult generationResult, GenShell shell)
		{
			//TODO: Control overwrites! What happend if the generated content already exits.
			//TODO: Control multiple primary outputs, continue on failure or abort?
			foreach (var output in generationResult.PrimaryOutputs)
			{
				if (!string.IsNullOrWhiteSpace(output.Path))
				{
					var projectPath = Path.GetFullPath(Path.Combine(outputPath, output.Path));
					shell.AddProjectToSolution(projectPath);
				}
			}
			return new PostActionResult()
			{
				ResultCode = ResultCode.Success,
				Message = PostActionResources.AddProjectToSolution_Success
			};
			
		}
	}
}
