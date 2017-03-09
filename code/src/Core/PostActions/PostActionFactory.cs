using Microsoft.TemplateEngine.Abstractions;
using Microsoft.TemplateEngine.Edge.Template;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.PostActions.Catalog;
using Microsoft.Templates.Core.PostActions.Catalog.Merge;
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
        public static IEnumerable<PostAction> Find(GenInfo genInfo, TemplateCreationResult genResult)
        {
            var postActions = new List<PostAction>();

            AddPredefinedActions(genInfo, genResult, postActions);
            AddMergeActions(genInfo,postActions);

            return postActions;
        }

        public static IEnumerable<PostAction> FindGlobal(List<GenInfo> genItems)
        {
            var postActions = new List<PostAction>();

            //TODO: REVIEW THIS FACTORY AND MAGIC STRINGS IN NAMES
            if (genItems.Any(g => g.Template.GetTemplateType() == TemplateType.DevFeature && g.Template.Name == "Localization"))
            {
                postActions.Add(new LocalizationPostAction());
            }
            else
            {
                postActions.Add(new CleanLocAnchorPostAction());
            }

            if (genItems.Any(g => g.Template.GetTemplateType() == TemplateType.DevFeature && g.Template.Name == "BackgroundTask"))
            {
                postActions.Add(new BackgroundTaskPostAction());
            }

            return postActions;
        }

        private static void AddPredefinedActions(GenInfo genInfo, TemplateCreationResult genResult, List<PostAction> postActions)
        {
            switch (genInfo.Template.GetTemplateType())
            {
                case TemplateType.Project:
                    postActions.Add(new AddProjectToSolutionPostAction( genResult.ResultInfo.PrimaryOutputs));
                    postActions.Add(new GenerateTestCertificatePostAction(genInfo.GetUserName()));
                    postActions.Add(new SetDefaultSolutionConfigurationPostAction());
                    break;
                case TemplateType.Page:
                    postActions.Add(new AddItemToProjectPostAction(genResult.ResultInfo.PrimaryOutputs));
                    break;
                case TemplateType.DevFeature:
                    postActions.Add(new AddItemToProjectPostAction(genResult.ResultInfo.PrimaryOutputs));
                    break;
                case TemplateType.ConsumerFeature:
                    postActions.Add(new AddItemToProjectPostAction(genResult.ResultInfo.PrimaryOutputs));
                    break;
                case TemplateType.Framework:
                    postActions.Add(new AddItemToProjectPostAction(genResult.ResultInfo.PrimaryOutputs));
                    break;
                default:
                    break;
            }
        }

        private static void AddMergeActions(GenInfo genInfo, List<PostAction> postActions)
        {
            Directory
               .EnumerateFiles(GenContext.Current.OutputPath, $"*{MergePostAction.Extension}*", SearchOption.AllDirectories)
               .ToList()
               .ForEach(f => postActions.Add(new MergePostAction(f)));
        }
    }
}
