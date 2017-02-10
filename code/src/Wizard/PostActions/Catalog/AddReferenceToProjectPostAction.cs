using Microsoft.TemplateEngine.Edge.Template;

namespace Microsoft.Templates.Wizard.PostActions.Catalog
{
    public class AddReferenceToProjectPostAction : PostActionBase
    {
        public AddReferenceToProjectPostAction() 
            : base("AddReferenceToProject", "This post action adds a reference of the generated project to the main project", null)
        {
        }

        public override PostActionResult Execute(string outputPath, GenInfo context, TemplateCreationResult generationResult, GenShell shell)
        {
            foreach (var output in generationResult.PrimaryOutputs)
            {
                if (!string.IsNullOrWhiteSpace(output.Path))
                {
                    shell.AddReferenceToProject(shell.ProjectName, context.Name);
                }
            }
            return new PostActionResult()
            {
                ResultCode = ResultCode.Success,
                Message = PostActionResources.AddReferenceToProject_Success
            };
        }
    }
}
