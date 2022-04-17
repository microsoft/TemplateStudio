// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Microsoft.Templates.Core.PostActions.Catalog.Merge
{
    public class MergeResult
    {
        public bool Success { get; set; }

        public IEnumerable<string> Result { get; set; }

        public string ErrorLine { get; set; }

        public int ErrorLineNumber { get; set; }
    }
}
