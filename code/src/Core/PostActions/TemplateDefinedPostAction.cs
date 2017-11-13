// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Resources;

namespace Microsoft.Templates.Core.PostActions
{
    public abstract class TemplateDefinedPostAction : PostAction
    {
        public abstract Guid ActionId { get; }

        public IReadOnlyDictionary<string, string> Args { get; private set; }

        public TemplateDefinedPostAction(string relatedTemplate, IPostAction templateDefinedPostAction)
            : base(relatedTemplate)
        {
            ContinueOnError = templateDefinedPostAction.ContinueOnError;

            if (IsIdsMatch(templateDefinedPostAction))
            {
                Args = templateDefinedPostAction.Args;
            }
        }

        private bool IsIdsMatch(IPostAction templateDefinedPostAction)
        {
            if (templateDefinedPostAction.ActionId != ActionId)
            {
                string errorMsg = string.Format(StringRes.PostActionIdsNotMatchError, templateDefinedPostAction.ActionId.ToString(), ActionId.ToString(), RelatedTemplate);
                if (!ContinueOnError)
                {
                    throw new Exception(errorMsg);
                }
                else
                {
                    AppHealth.Current.Error.TrackAsync(errorMsg).FireAndForget();
                    return false;
                }
            }

            return true;
        }
    }
}
