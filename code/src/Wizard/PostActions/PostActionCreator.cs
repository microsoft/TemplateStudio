using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Newtonsoft.Json;

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
					postActions.Add(GetPostAction(PostActionType.AddProjectToSolution));
					postActions.Add(GetPostAction(PostActionType.GenerateTestCertificatePostAction));
					break;
				case TemplateType.Page:
					postActions.Add(GetPostAction(PostActionType.AddItemToProject));
					break;
				case TemplateType.Feature:
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

				case PostActionType.AddProjectToSolution:
					return new AddProjectToSolutionPostAction();

				case PostActionType.AddItemToProject:
					return new AddItemToProjectPostAction();

				default:
					return null;
			}
		}
	}
}
