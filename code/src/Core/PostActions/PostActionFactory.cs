// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.TemplateEngine.Edge.Template;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.PostActions.Catalog;
using Microsoft.Templates.Core.PostActions.Catalog.Merge;
using Microsoft.Templates.Core.PostActions.Catalog.SortUsings;

namespace Microsoft.Templates.Core.PostActions
{
    public static class PostActionFactory
    {
        public static IEnumerable<PostAction> Find(GenInfo genInfo, TemplateCreationResult genResult)
        {
            var postActions = new List<PostAction>();

            AddPredefinedActions(genInfo, genResult, postActions);
            AddMergeActions(postActions, $"*{MergePostAction.Extension}*");

            return postActions;
        }

        public static IEnumerable<PostAction> FindGlobal(List<GenInfo> genItems)
        {
            var postActions = new List<PostAction>();

            AddMergeActions(postActions, $"*{MergePostAction.GlobalExtension}*");

            postActions.Add(new SetDefaultSolutionConfigurationPostAction());
            postActions.Add(new SortUsingsPostAction());

            return postActions;
        }

        private static void AddPredefinedActions(GenInfo genInfo, TemplateCreationResult genResult, List<PostAction> postActions)
        {
            switch (genInfo.Template.GetTemplateType())
            {
                case TemplateType.Project:
                    postActions.Add(new AddProjectToSolutionPostAction( genResult.ResultInfo.PrimaryOutputs));
                    postActions.Add(new GenerateTestCertificatePostAction(genInfo.GetUserName()));
                    break;
                case TemplateType.Page:
                    postActions.Add(new AddItemToProjectPostAction(genResult.ResultInfo.PrimaryOutputs));
                    break;
                case TemplateType.Feature:
                    postActions.Add(new AddItemToProjectPostAction(genResult.ResultInfo.PrimaryOutputs));
                    break;
                case TemplateType.Composition:
                    postActions.Add(new AddItemToProjectPostAction(genResult.ResultInfo.PrimaryOutputs));
                    break;
                default:
                    break;
            }
        }

        private static void AddMergeActions(List<PostAction> postActions, string searchPattern)
        {
            Directory
               .EnumerateFiles(GenContext.Current.OutputPath, searchPattern, SearchOption.AllDirectories)
               .ToList()
               .ForEach(f => postActions.Add(new MergePostAction(f)));
        }
    }
}
