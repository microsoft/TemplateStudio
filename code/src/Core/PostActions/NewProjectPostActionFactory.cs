// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

using Microsoft.TemplateEngine.Edge.Template;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.PostActions.Catalog;
using Microsoft.Templates.Core.PostActions.Catalog.Merge;
using Microsoft.Templates.Core.PostActions.Catalog.SortNamespaces;

namespace Microsoft.Templates.Core.PostActions
{
    public class NewProjectPostActionFactory : PostActionFactory
    {
        public override IEnumerable<PostAction> FindPostActions(GenInfo genInfo, TemplateCreationResult genResult)
        {
            var postActions = new List<PostAction>();

            AddPredefinedActions(genInfo, genResult, postActions);
            AddTemplateDefinedPostActions(genInfo, genResult, postActions);
            AddMergeActions(genInfo, postActions, $"*{MergeConfiguration.Extension}*", true);
            AddSearchAndReplaceActions(genInfo, postActions, $"*{SearchAndReplacePostAction.Extension}*");

            return postActions;
        }

        public override IEnumerable<PostAction> FindGlobalPostActions()
        {
            var postActions = new List<PostAction>();

            AddGlobalMergeActions(postActions, $"*{MergeConfiguration.GlobalExtension}*", true);
            postActions.Add(new SortUsingsPostAction());
            postActions.Add(new SortImportsPostAction());
            postActions.Add(new AddContextItemsToProjectPostAction());
            postActions.Add(new SetDefaultSolutionConfigurationPostAction());

            return postActions;
        }
    }
}
