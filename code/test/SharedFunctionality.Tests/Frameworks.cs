// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Microsoft.Templates.Test
{
    public static class Frameworks
    {
        public const string CodeBehind = "CodeBehind";

        public const string MVVMToolkit = "MVVMToolkit";

        public const string None = "None";

        public const string Prism = "Prism";

        public static IEnumerable<string> All => new List<string> { CodeBehind, MVVMToolkit, Prism };
    }
}
