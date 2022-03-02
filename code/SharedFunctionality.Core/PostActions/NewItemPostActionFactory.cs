// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.IO;
using Microsoft.TemplateEngine.Edge.Template;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.PostActions.Catalog;
using Microsoft.Templates.Core.PostActions.Catalog.Merge;
using Microsoft.Templates.Core.PostActions.Catalog.SortNamespaces;

namespace Microsoft.Templates.Core.PostActions
{
    public class NewItemPostActionFactory : PostActionFactory
    {
        public override IEnumerable<PostAction> FindPostActions(GenInfo genInfo, ITemplateCreationResult genResult)
        {
            var postActions = new List<PostAction>();

            AddPredefinedActions(genInfo, genResult, postActions);
            AddTemplateDefinedPostActions(genInfo, genResult, postActions);
            AddGetMergeFilesFromProjectPostAction(genInfo, postActions);
            AddGenerateMergeInfoPostAction(genInfo, postActions);
            AddMergeActions(genInfo, postActions, $"*{MergeConfiguration.Extension}*", false);
            AddSearchAndReplaceActions(genInfo, postActions, $"*{MergeConfiguration.SearchReplaceExtension}*", false);

            return postActions;
        }

        public override IEnumerable<PostAction> FindGlobalPostActions()
        {
            var postActions = new List<PostAction>();

            AddGlobalMergeActions(postActions, $"*{MergeConfiguration.GlobalExtension}*", false);

            var paths = new List<string>() { Path.GetDirectoryName(GenContext.Current.GenerationOutputPath) };

            postActions.Add(new SortUsingsPostAction(paths));
            postActions.Add(new SortImportsPostAction(paths));

            return postActions;
        }

        public override IEnumerable<PostAction> FindSyncGenerationPostActions(TempGenerationResult result)
        {
            var postActions = new List<PostAction>();

            postActions.Add(new CopyFilesToProjectPostAction(result));
            postActions.Add(new AddContextItemsToSolutionAndProjectPostAction());
            postActions.Add(new CreateSummaryPostAction(result));
            postActions.Add(new OpenFilesPostAction());

            return postActions;
        }

        public override IEnumerable<PostAction> FindOutputGenerationPostActions(TempGenerationResult result)
        {
            var postActions = new List<PostAction>();

            postActions.Add(new CreateSummaryPostAction(result));
            postActions.Add(new OpenFilesPostAction());

            return postActions;
        }
    }
}
