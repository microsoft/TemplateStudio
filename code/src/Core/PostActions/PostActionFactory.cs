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
using Microsoft.Templates.Core.PostActions.Catalog.SortNamespaces;
using System.Reflection;

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

        internal void AddGetMergeFilesFromProjectPostAction(List<PostAction> postActions)
        {
            Directory
                .EnumerateFiles(GenContext.Current.OutputPath, "*.*", SearchOption.AllDirectories)
                .Where(f => Regex.IsMatch(f, MergeConfiguration.PostactionRegex))
                .ToList()
                .ForEach(f => postActions.Add(new GetMergeFilesFromProjectPostAction(f)));
        }

        internal void AddGenerateMergeInfoPostAction(List<PostAction> postActions)
        {
            Directory
                .EnumerateFiles(GenContext.Current.OutputPath, "*.*", SearchOption.AllDirectories)
                .Where(f => Regex.IsMatch(f, MergeConfiguration.PostactionRegex))
                .ToList()
                .ForEach(f => postActions.Add(new GenerateMergeInfoPostAction(f)));
        }

        internal void AddTemplateDefinedPostActions(GenInfo genInfo, TemplateCreationResult genResult, List<PostAction> postActions)
        {
            var genCertificatePostAction = genResult.ResultInfo.PostActions.Where(x => x.ActionId == GenerateTestCertificatePostAction.Id).FirstOrDefault();
            if (genCertificatePostAction != null)
            {
                postActions.Add(new GenerateTestCertificatePostAction(genInfo.GetUserName(), genCertificatePostAction, genResult.ResultInfo.PrimaryOutputs));
            }
        }

        internal void AddPredefinedActions(GenInfo genInfo, TemplateCreationResult genResult, List<PostAction> postActions)
        {
            switch (genInfo.Template.GetTemplateType())
            {
                case TemplateType.Project:
                    postActions.Add(new AddProjectToSolutionPostAction(genResult.ResultInfo.PrimaryOutputs));
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

        internal void AddMergeActions(List<PostAction> postActions, string searchPattern, bool failOnError)
        {
            Directory
                .EnumerateFiles(GenContext.Current.OutputPath, searchPattern, SearchOption.AllDirectories)
                .ToList()
                .ForEach(f => AddMergePostAction(postActions, failOnError, f));
        }

        internal void AddGlobalMergeActions(List<PostAction> postActions, string searchPattern, bool failOnError)
        {
            Directory
                .EnumerateFiles(GenContext.Current.OutputPath, searchPattern, SearchOption.AllDirectories)
                .ToList()
                .ForEach(f => postActions.Add(new MergePostAction(new MergeConfiguration(f, failOnError))));
        }

        internal void AddSearchAndReplaceActions(List<PostAction> postActions, string searchPattern)
        {
            Directory
                .EnumerateFiles(GenContext.Current.OutputPath, searchPattern, SearchOption.AllDirectories)
                .ToList()
                .ForEach(f => postActions.Add(new SearchAndReplacePostAction(f)));
        }

        private static void AddMergePostAction(List<PostAction> postActions, bool failOnError, string f)
        {
            if (IsResourceDictionaryPostaction(f))
            {
                postActions.Add(new MergeResourceDictionaryPostAction(new MergeConfiguration(f, failOnError)));
            }
            else
            {
                postActions.Add(new MergePostAction(new MergeConfiguration(f, failOnError)));
            }
        }

        private static bool IsResourceDictionaryPostaction(string f)
        {
            return Path.GetExtension(f).ToLower() == ".xaml" & File.ReadAllText(f).StartsWith(MergeConfiguration.ResourceDictionaryMatch);
        }
    }
}
