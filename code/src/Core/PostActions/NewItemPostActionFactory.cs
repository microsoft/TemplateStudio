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
    public class NewItemPostActionFactory : PostActionFactory
    {
        public override IEnumerable<PostAction> FindPostActions(GenInfo genInfo, TemplateCreationResult genResult)
        {
            var postActions = new List<PostAction>();

            AddGetMergeFilesFromProjectPostAction(postActions);
            AddGenerateMergeInfoPostAction(postActions);
            AddMergeActions(postActions, $"*{MergePostAction.Extension}*", false);

            return postActions;
        }

        public override IEnumerable<PostAction> FindGlobalPostActions()
        {
            var postActions = new List<PostAction>();

            AddGlobalMergeActions(postActions, $"*{MergePostAction.GlobalExtension}*", false);
            postActions.Add(new SortUsingsPostAction());
            postActions.Add(new CompareTempGenerationWithProjectPostAction());

            return postActions;
        }

        public override IEnumerable<PostAction> FindSyncGenerationPostActions()
        {
            var postActions = new List<PostAction>();

            postActions.Add(new CopyFilesToProjectPostAction());
            postActions.Add(new AddContextItemsToProjectPostAction());
            postActions.Add(new CreateSyncSummaryPostAction());
            postActions.Add(new OpenFilesPostAction());

            return postActions;
        }

        public override IEnumerable<PostAction> FindOutputGenerationPostActions()
        {
            var postActions = new List<PostAction>();

            postActions.Add(new CreateSyncStepsInstructionsPostAction());
            postActions.Add(new OpenFilesPostAction());

            return postActions;
        }
    }
}
