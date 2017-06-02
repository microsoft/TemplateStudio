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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.TemplateEngine.Edge.Template;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.PostActions.Catalog;
using Microsoft.Templates.Core.PostActions.Catalog.Merge;
using Microsoft.Templates.Core.PostActions.Catalog.SortUsings;
using System.Text.RegularExpressions;

namespace Microsoft.Templates.Core.PostActions
{
    public static class PostActionFactory
    {
        public static IEnumerable<PostAction> FindNewProjectPostActions(GenInfo genInfo, TemplateCreationResult genResult)
        {
            var postActions = new List<PostAction>();

            AddPredefinedActions(genInfo, genResult, postActions);
            AddMergeActions(postActions, $"*{MergePostAction.Extension}*", true);

            return postActions;
        }

        public static IEnumerable<PostAction> FindNewItemPostActions(GenInfo genInfo, TemplateCreationResult genResult)
        {
            var postActions = new List<PostAction>();

            AddPredefinedActions(genInfo, genResult, postActions);           
            AddGetMergeFilesFromProjectPostAction(postActions);
            AddMergeActions(postActions, $"*{MergePostAction.Extension}*", false);

            return postActions;
        }

        public static IEnumerable<PostAction> FindGlobalNewProjectPostActions()
        {
            var postActions = new List<PostAction>();

            AddGlobalMergeActions(postActions, $"*{MergePostAction.GlobalExtension}*", true);
            postActions.Add(new SortUsingsPostAction());
            postActions.Add(new AddContextItemsToProjectPostAction());
            postActions.Add(new SetDefaultSolutionConfigurationPostAction());

            return postActions;
        }

        public static IEnumerable<PostAction> FindGlobalNewItemPostActions()
        {
            var postActions = new List<PostAction>();

            AddGlobalMergeActions(postActions, $"*{MergePostAction.GlobalExtension}*", false);
            postActions.Add(new SortUsingsPostAction());

            return postActions;
        }

        public static IEnumerable<PostAction> FindFinishItemGenerationPostActions()
        {
            var postActions = new List<PostAction>();

            postActions.Add(new AddContextItemsToProjectPostAction());

            return postActions;
        }

        private static void AddGetMergeFilesFromProjectPostAction(List<PostAction> postActions)
        {
            Directory
                .EnumerateFiles(GenContext.Current.OutputPath, "*.*", SearchOption.AllDirectories)
                .Where(f => Regex.IsMatch(f, GetMergeFilesFromProjectPostAction.PostactionRegex))
                .ToList()
                .ForEach(f => postActions.Add(new GetMergeFilesFromProjectPostAction(f)));
            
        }

        private static void AddPredefinedActions(GenInfo genInfo, TemplateCreationResult genResult, List<PostAction> postActions)
        {
            switch (genInfo.Template.GetTemplateType())
            {
                case TemplateType.Project:
                    postActions.Add(new AddProjectToSolutionPostAction(genResult.ResultInfo.PrimaryOutputs));
                    postActions.Add(new GenerateTestCertificatePostAction(genInfo.GetUserName()));
                    break;
                case TemplateType.Page:
                case TemplateType.Feature:
                case TemplateType.Composition:
                    postActions.Add(new AddItemToContextPostAction(genResult.ResultInfo.PrimaryOutputs));
                    break;
                default:
                    break;
            }
        }

        private static void AddMergeActions(List<PostAction> postActions, string searchPattern, bool failOnError)
        {
            Directory
                .EnumerateFiles(GenContext.Current.OutputPath, searchPattern, SearchOption.AllDirectories)
                .ToList()
                .ForEach(f => postActions.Add(new MergePostAction(new MergeConfiguration(f, failOnError))));
        }



        private static void AddGlobalMergeActions(List<PostAction> postActions, string searchPattern, bool failOnError)
        {
            Directory
                .EnumerateFiles(GenContext.Current.OutputPath, searchPattern, SearchOption.AllDirectories)
                .ToList()
                .ForEach(f => postActions.Add(new MergePostAction(new MergeConfiguration(f, failOnError))));
        }
    }
}
