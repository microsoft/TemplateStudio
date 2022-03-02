// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Microsoft.Templates.Core.PostActions.Catalog.SortNamespaces
{
    public class SortUsingsPostAction : SortNamespacesPostAction
    {
        public SortUsingsPostAction(List<string> paths)
            : base(paths)
        {
        }

        public override string FilesToSearch => "*.cs";

        public override bool SortMethod(List<string> classContent)
        {
            return classContent.SortUsings();
        }
    }
}
