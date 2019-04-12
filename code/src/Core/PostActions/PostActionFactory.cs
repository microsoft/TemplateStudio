// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.TemplateEngine.Edge.Template;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.PostActions.Catalog;
using Microsoft.Templates.Core.PostActions.Catalog.AddJsonDictionaryItem;
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
                .EnumerateFiles(genInfo.GenerationPath, "*.*", SearchOption.AllDirectories)
                .Where(f => Regex.IsMatch(f, MergeConfiguration.PostactionAndSearchReplaceRegex))
                .ToList()
                .ForEach(f => postActions.Add(new GetMergeFilesFromProjectPostAction(genInfo.Template.Identity, f)));
        }

        internal void AddGenerateMergeInfoPostAction(GenInfo genInfo, List<PostAction> postActions)
        {
            Directory
                .EnumerateFiles(Path.GetDirectoryName(genInfo.GenerationPath), "*.*", SearchOption.AllDirectories)
                .Where(f => Regex.IsMatch(f, MergeConfiguration.PostactionRegex) || Regex.IsMatch(f, MergeConfiguration.PostactionAndSearchReplaceRegex))
                .ToList()
                .ForEach(f => postActions.Add(new GenerateMergeInfoPostAction(genInfo.Template.Identity, f)));
        }

        internal void AddTemplateDefinedPostActions(GenInfo genInfo, TemplateCreationResult genResult, List<PostAction> postActions)
        {
            foreach (var postaction in genResult.ResultInfo.PostActions)
            {
                switch (postaction.ActionId.ToString().ToUpper(CultureInfo.InvariantCulture))
                {
                    case GenerateTestCertificatePostAction.Id:
                        postActions.Add(new GenerateTestCertificatePostAction(genInfo.Template.Identity, genInfo.GetUserName(), postaction, genResult.ResultInfo.PrimaryOutputs, genInfo.Parameters, genInfo.DestinationPath));
                        break;
                    case AddProjectReferencesToContextPostAction.Id:
                        postActions.Add(new AddProjectReferencesToContextPostAction(genInfo.Template.Identity, postaction, genResult.ResultInfo.PrimaryOutputs, genInfo.Parameters, genInfo.DestinationPath));
                        break;
                    case AddNugetReferenceToContextPostAction.Id:
                        postActions.Add(new AddNugetReferenceToContextPostAction(genInfo.Template.Identity, postaction, genInfo.Parameters, genInfo.DestinationPath));
                        break;
                    case AddJsonDictionaryItemPostAction.Id:
                        postActions.Add(new AddJsonDictionaryItemPostAction(genInfo.Template.Identity, postaction, genInfo.Parameters, genInfo.DestinationPath));
                        break;
                    case AddSdkReferencesToContextPostAction.Id:
                        postActions.Add(new AddSdkReferencesToContextPostAction(genInfo.Template.Identity, postaction, genInfo.Parameters, genInfo.DestinationPath));
                        break;
                }
            }
        }

        internal void AddPredefinedActions(GenInfo genInfo, TemplateCreationResult genResult, List<PostAction> postActions)
        {
            switch (genInfo.Template.GetTemplateOutputType())
            {
                case TemplateOutputType.Project:
                    postActions.Add(new AddProjectToContextPostAction(genInfo.Template.Identity, genResult.ResultInfo.PrimaryOutputs, genInfo.Parameters, genInfo.DestinationPath, genInfo.GenerationPath));
                    break;
                case TemplateOutputType.Item:
                    postActions.Add(new AddItemToContextPostAction(genInfo.Template.Identity, genResult.ResultInfo.PrimaryOutputs, genInfo.Parameters, genInfo.DestinationPath));
                    break;
                default:
                    break;
            }
        }

        internal void AddMergeActions(GenInfo genInfo, List<PostAction> postActions, string searchPattern, bool failOnError)
        {
            var files = Directory
                .EnumerateFiles(genInfo.GenerationPath, searchPattern, SearchOption.AllDirectories)
                .ToList();
            files.ForEach(f => AddMergePostAction(genInfo, postActions, failOnError, f));
        }

        internal void AddGlobalMergeActions(List<PostAction> postActions, string searchPattern, bool failOnError)
        {
            Directory
                .EnumerateFiles(Path.GetDirectoryName(GenContext.Current.GenerationOutputPath), searchPattern, SearchOption.AllDirectories)
                .ToList()
                .ForEach(f => postActions.Add(new MergePostAction("Global Merge", new MergeConfiguration(f, failOnError))));
        }

        internal void AddSearchAndReplaceActions(GenInfo genInfo, List<PostAction> postActions, string searchPattern, bool failOnError)
        {
            Directory
                .EnumerateFiles(genInfo.GenerationPath, searchPattern, SearchOption.AllDirectories)
                .ToList()
                .ForEach(f => postActions.Add(new SearchAndReplacePostAction(genInfo.Template.Identity, new MergeConfiguration(f, failOnError))));
        }

        private static void AddMergePostAction(GenInfo genInfo, List<PostAction> postActions, bool failOnError, string f)
        {
            if (IsResourceDictionaryPostaction(f))
            {
                postActions.Add(new MergeResourceDictionaryPostAction(genInfo.Template.Identity, new MergeConfiguration(f, failOnError)));
            }
            else
            {
                postActions.Add(new MergePostAction(genInfo.Template.Identity, new MergeConfiguration(f, failOnError)));
            }
        }

        private static bool IsResourceDictionaryPostaction(string f)
        {
            return Path.GetExtension(f).Equals(".xaml", StringComparison.OrdinalIgnoreCase) && File.ReadAllText(f).StartsWith(MergeConfiguration.ResourceDictionaryMatch, StringComparison.Ordinal);
        }
    }
}
