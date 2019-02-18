// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core.Composition;

namespace Microsoft.Templates.Core.Gen
{
    public class CompositionInfo
    {
        public CompositionQuery Query { get; set; }

        public ITemplateInfo Template { get; set; }
    }
}
