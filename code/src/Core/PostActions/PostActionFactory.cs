using Microsoft.TemplateEngine.Abstractions;
using Microsoft.TemplateEngine.Edge.Template;
using Microsoft.Templates.Core.PostActions.Catalog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.PostActions
{
    public static class PostActionFactory
    {
        public static IEnumerable<PostAction> Find(GenShell shell, GenInfo genInfo, TemplateCreationResult genResult)
        {
            var postActions = new List<PostAction>();

            AddPredefinedActions(shell, genInfo, genResult, postActions);
            AddInjectionActions(shell, postActions);

            return postActions;
        }

        public static IEnumerable<PostAction> FindGlobal(GenShell shell, List<GenInfo> genItems)
        {
            var postActions = new List<PostAction>();

            //TODO: REVIEW THIS FACTORY AND MAGIC STRINGS IN NAMES
            if (genItems.Any(g => g.Template.GetTemplateType() == TemplateType.DevFeature && g.Template.Name == "Localization"))
            {
                postActions.Add(new LocalizationPostAction(shell));
            }
            else
            {
                postActions.Add(new CleanLocAnchorPostAction(shell));
            }

            if (genItems.Any(g => g.Template.GetTemplateType() == TemplateType.DevFeature && g.Template.Name == "BackgroundTask"))
            {
                postActions.Add(new BackgroundTaskPostAction(shell));
            }

            return postActions;
        }

        private static void AddPredefinedActions(GenShell shell, GenInfo genInfo, TemplateCreationResult genResult, List<PostAction> postActions)
        {
            switch (genInfo.Template.GetTemplateType())
            {
                case TemplateType.Project:
                    postActions.Add(new AddProjectToSolutionPostAction(shell, genResult.ResultInfo.PrimaryOutputs));
                    postActions.Add(new GenerateTestCertificatePostAction(shell, genInfo.GetUserName()));
                    postActions.Add(new SetDefaultSolutionConfigurationPostAction(shell));
                    break;
                case TemplateType.Page:
                    postActions.Add(new AddItemToProjectPostAction(shell, genResult.ResultInfo.PrimaryOutputs));
                    break;
                case TemplateType.DevFeature:
                    postActions.Add(new AddItemToProjectPostAction(shell, genResult.ResultInfo.PrimaryOutputs));
                    break;
                case TemplateType.ConsumerFeature:
                    postActions.Add(new AddItemToProjectPostAction(shell, genResult.ResultInfo.PrimaryOutputs));
                    break;
                case TemplateType.Framework:
                    postActions.Add(new AddItemToProjectPostAction(shell, genResult.ResultInfo.PrimaryOutputs));
                    break;
                default:
                    break;
            }
        }

        private static void AddInjectionActions(GenShell shell, List<PostAction> postActions)
        {
             Directory
                .EnumerateFiles(shell.ProjectPath, "*.postaction", SearchOption.AllDirectories)
                .ToList()
                .ForEach(f => postActions.Add(new InjectionPostAction(shell, f)));
        }
    }
}
