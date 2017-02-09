using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.Wizard.PostActions.Catalog;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Microsoft.Templates.Wizard.PostActions
{
    public class PostActionCreator
	{

		public static IEnumerable<PostActionBase> GetPostActions(ITemplateInfo template)
		{
			var postActions = new List<PostActionBase>();


			switch (template.GetTemplateType())
			{
				case TemplateType.Project:
					postActions.Add(GetPostAction(PostActionType.AddProjectToSolutionPostAction));
					postActions.Add(GetPostAction(PostActionType.GenerateTestCertificatePostAction));
                    postActions.Add(GetPostAction(PostActionType.SetDefaultSolutionConfigurationPostAction));
                    break;
				case TemplateType.Page:
					postActions.Add(GetPostAction(PostActionType.AddItemToProjectPostAction));
					break;
				case TemplateType.Feature:
                    if (template.GetTemplateOutputType() == "project")
                    {
                        postActions.Add(GetPostAction(PostActionType.AddProjectToSolutionPostAction));
                    }
					break;
				case TemplateType.Unspecified:
					break;
				default:
					break;
			}

			var customPostActionConfigFile = template.GetPostActionConfigPath();

			if (!string.IsNullOrWhiteSpace(customPostActionConfigFile))
			{
				var json = File.ReadAllText(customPostActionConfigFile);
				var customPostActions = JsonConvert.DeserializeObject<CustomPostActionDefinition[]>(json).ToList();

				postActions.AddRange(customPostActions.Select(p => PostActionCreator.GetCustomPostAction(p)));
			}

			return postActions;
		}

		private static PostActionBase GetPostAction(PostActionType postActionType)
		{
			return GetPostAction(postActionType, null, null, null);
		}

		private static PostActionBase GetCustomPostAction(CustomPostActionDefinition postActionDefinition)
		{
			return GetPostAction(postActionDefinition.Type, postActionDefinition.Name, postActionDefinition.Description, postActionDefinition.Parameters);
		}

		private static PostActionBase GetPostAction(PostActionType postActionType, string name, string description, IReadOnlyDictionary<string, string> parameters)
		{
            switch (postActionType)
            {
                case PostActionType.InsertPartialGenerationPostAction:
                    return new InsertPartialGenerationPostAction(name, description, parameters);

                case PostActionType.GenerateTestCertificatePostAction:
                    return new GenerateTestCertificatePostAction();

                case PostActionType.AddProjectToSolutionPostAction:
                    return new AddProjectToSolutionPostAction();

                case PostActionType.AddItemToProjectPostAction:
                    return new AddItemToProjectPostAction();

                case PostActionType.SetDefaultSolutionConfigurationPostAction:
                    return new SetDefaultSolutionConfigurationPostAction();
                    
                default:
                    return null;
            }
        }

        public static void CleanUpAnchors(string outputPath)
        {
            string[] anchorTexts = { "//PostActionAnchor", "<!--PostActionAnchor" };

            var projectFiles = Directory.EnumerateFiles(outputPath, "*", SearchOption.AllDirectories);

            foreach (var file in projectFiles)
            {
                var modified = false;
                var fileContent = File.ReadAllText(file);
                foreach (var anchorText in anchorTexts)
                {
                    var anchorIndex = 0;
                    //Search the whole file, until nothing else is found
                    while (anchorIndex != -1)
                    {
                        anchorIndex = fileContent.IndexOf(anchorText, StringComparison.Ordinal);
                        if (anchorIndex != -1)
                        {
                            var nextLineBreakAfterAnchor = fileContent.IndexOf(Environment.NewLine, anchorIndex, StringComparison.Ordinal);

                            fileContent = fileContent.Remove(anchorIndex, nextLineBreakAfterAnchor - anchorIndex);
                            modified = true;
                        }
                    }
                }

                if (modified) File.WriteAllText(file, fileContent);

            }
        }
    }
}
