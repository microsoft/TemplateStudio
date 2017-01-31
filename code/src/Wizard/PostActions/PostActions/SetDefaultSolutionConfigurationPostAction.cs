using System;
using Microsoft.TemplateEngine.Edge.Template;
using Microsoft.Templates.Wizard.Resources;

namespace Microsoft.Templates.Wizard.PostActions
{
    public class SetDefaultSolutionConfigurationPostAction : PostActionBase
    {
        
        public SetDefaultSolutionConfigurationPostAction() 
            : base("SetDefaultSolutionConfiguration", "This post action sets the default solution configuration", null)	
        {
        }

        private const string Configuration = "Debug";
        private const string Platform = "x86";

        public override PostActionResult Execute(GenInfo context, TemplateCreationResult generationResult, GenShell shell)
        {
            try
            {
                
                var result = shell.SetActiveConfigurationAndPlatform(Configuration, Platform);
                if (result == true)
                {
                    return new PostActionResult()
                    {
                        ResultCode = ResultCode.Success,
                        Message = PostActionResources.SetDefaultSolutionConfiguration_Success
                    };
                }
                else
                {
                    return new PostActionResult()
                    {
                        ResultCode = ResultCode.ContextError,
                        Message = PostActionResources.SetDefaultSolutionConfiguration_ConfigurationNotFoundPattern.UseParams(Configuration, Platform)
                    };
                }
            }
            catch (Exception ex)
            {
                return new PostActionResult()
                {
                    ResultCode = ResultCode.Error,
                    Message = PostActionResources.SetDefaultSolutionConfiguration_Error,
                    Exception = ex
                };
            }
        }
    }
}
