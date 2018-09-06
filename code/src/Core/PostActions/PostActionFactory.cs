// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.TemplateEngine.Edge.Template;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.PostActions.Catalog;
using Microsoft.Templates.Core.PostActions.Catalog.Merge;

namespace Microsoft.Templates.Core.PostActions
{
    public abstract class PostActionFactory
    {
        public abstract IEnumerable<PostAction> FindPostActions(GenInfo genInfo, TemplateCreationResult genResult);

        public abstract IEnumerable<PostAction> FindGlobalPostActions();

        public virtual IEnumerable<PostAction> FindSyncGenerationPostActions(TempGenerationResult result)
        {
            return new List<PostAction>();
        }

        public virtual IEnumerable<PostAction> FindOutputGenerationPostActions(TempGenerationResult result)
        {
            return new List<PostAction>();
        }

        internal void AddGetMergeFilesFromProjectPostAction(GenInfo genInfo, List<PostAction> postActions)
        {
            Directory
                .EnumerateFiles(GenContext.Current.OutputPath, "*.*", SearchOption.AllDirectories)
                .Where(f => Regex.IsMatch(f, MergeConfiguration.PostactionAndSearchReplaceRegex))
                .ToList()
                .ForEach(f => postActions.Add(new GetMergeFilesFromProjectPostAction(genInfo.Template.Identity, f)));
        }

        internal void AddGenerateMergeInfoPostAction(GenInfo genInfo, List<PostAction> postActions)
        {
            Directory
                .EnumerateFiles(Path.GetDirectoryName(GenContext.Current.OutputPath), "*.*", SearchOption.AllDirectories)
                .Where(f => Regex.IsMatch(f, MergeConfiguration.PostactionRegex))
                .ToList()
                .ForEach(f => postActions.Add(new GenerateMergeInfoPostAction(genInfo.Template.Identity, f)));
        }

        internal void AddTemplateDefinedPostActions(GenInfo genInfo, TemplateCreationResult genResult, List<PostAction> postActions)
        {
            var genCertificatePostAction = genResult.ResultInfo.PostActions.FirstOrDefault(x => x.ActionId == GenerateTestCertificatePostAction.Id);
            if (genCertificatePostAction != null)
            {
                postActions.Add(new GenerateTestCertificatePostAction(genInfo.Template.Identity, genInfo.GetUserName(), genCertificatePostAction, genResult.ResultInfo.PrimaryOutputs, genInfo.Parameters));
            }

            var addProjectReferencePostAction = genResult.ResultInfo.PostActions.FirstOrDefault(x => x.ActionId == AddReferenceToProjectPostAction.Id);
            if (addProjectReferencePostAction != null)
            {
                postActions.Add(new AddReferenceToProjectPostAction(genInfo.Template.Identity, addProjectReferencePostAction, genResult.ResultInfo.PrimaryOutputs, genInfo.Parameters));
            }
        }

        internal void AddPredefinedActions(GenInfo genInfo, TemplateCreationResult genResult, List<PostAction> postActions, bool addingToExistingProject = false)
        {
            switch (genInfo.Template.GetTemplateOutputType())
            {
                case TemplateOutputType.Project:
                    postActions.Add(new AddProjectToSolutionPostAction(genInfo.Template.Identity, genResult.ResultInfo.PrimaryOutputs, genInfo.Parameters));
                    break;
                case TemplateOutputType.Item:
                    postActions.Add(new AddItemToContextPostAction(genInfo.Template.Identity, genResult.ResultInfo.PrimaryOutputs, genInfo.Parameters));
                    break;
                default:
                    break;
            }
        }

        internal void AddMergeActions(GenInfo genInfo, List<PostAction> postActions, string searchPattern, bool failOnError)
        {
            Directory
                .EnumerateFiles(GenContext.Current.OutputPath, searchPattern, SearchOption.AllDirectories)
                .ToList()
                .ForEach(f => AddMergePostAction(genInfo, postActions, failOnError, f));
        }

        internal void AddGlobalMergeActions(List<PostAction> postActions, string searchPattern, bool failOnError)
        {
            Directory
                .EnumerateFiles(Path.GetDirectoryName(GenContext.Current.OutputPath), searchPattern, SearchOption.AllDirectories)
                .ToList()
                .ForEach(f => postActions.Add(new MergePostAction("Global Merge", new MergeConfiguration(f, failOnError, outputtingToParent: true))));
        }

        internal void AddSearchAndReplaceActions(GenInfo genInfo, List<PostAction> postActions, string searchPattern, bool failOnError)
        {
            Directory
                .EnumerateFiles(GenContext.Current.OutputPath, searchPattern, SearchOption.AllDirectories)
                .ToList()
                .ForEach(f => postActions.Add(new SearchAndReplacePostAction(genInfo.Template.Identity, new MergeConfiguration(f, failOnError, outputtingToParent: false))));
        }

        private static void AddMergePostAction(GenInfo genInfo, List<PostAction> postActions, bool failOnError, string f)
        {
            if (IsResourceDictionaryPostaction(f))
            {
                postActions.Add(new MergeResourceDictionaryPostAction(genInfo.Template.Identity, new MergeConfiguration(f, failOnError, genInfo.Template.GetOutputToParent())));
            }
            else
            {
                postActions.Add(new MergePostAction(genInfo.Template.Identity, new MergeConfiguration(f, failOnError, genInfo.Template.GetOutputToParent())));
            }
        }

        private static bool IsResourceDictionaryPostaction(string f)
        {
            return Path.GetExtension(f).Equals(".xaml", StringComparison.OrdinalIgnoreCase) && File.ReadAllText(f).StartsWith(MergeConfiguration.ResourceDictionaryMatch, StringComparison.Ordinal);
        }
    }
}
