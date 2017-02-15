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
                case TemplateType.DevFeature:
                    postActions.Add(GetPostAction(PostActionType.AddItemToProjectPostAction));
                    break;
                case TemplateType.Framework:
                    postActions.Add(GetPostAction(PostActionType.AddItemToProjectPostAction));
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

                case PostActionType.LocalizationPostAction:
                    return new LocalizationPostAction();

                default:
                    return null;
            }
        }

        public static void CleanUpAnchors(string outputPath)
        {
            var projectFiles = Directory.EnumerateFiles(outputPath, "*", SearchOption.AllDirectories);

            foreach (var file in projectFiles)
            {
                var fileContent = File.ReadAllText(file);

                var modified = Catalog.InsertPartialGenerationPostAction.CleanUpAnchors(ref fileContent);
                var modifiedLoc = Catalog.LocalizationPostAction.CleanUpAnchors(ref fileContent);

                if (modified || modifiedLoc)
                {
                    File.WriteAllText(file, fileContent);
                }

            }
        }
    }
}
