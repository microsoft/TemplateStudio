// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Resources;

namespace Microsoft.Templates.Core.PostActions
{
    public abstract class TemplateDefinedPostAction : PostAction
    {
        public virtual Guid ActionId { get; }

        public IReadOnlyDictionary<string, string> Args { get; private set; }

        public TemplateDefinedPostAction(string relatedTemplate, IPostAction templateDefinedPostAction)
        {
            RelatedTemplate = relatedTemplate;
            ContinueOnError = templateDefinedPostAction.ContinueOnError;
            Args = templateDefinedPostAction.Args;
        }
    }
}
