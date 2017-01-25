using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

			if (template.GetTemplateType() == TemplateType.Project)
			{
				postActions.Add(GetPostAction(PostActionType.GenerateTestCertificatePostAction));
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

				default:
					return null;
			}
		}
	}
}
