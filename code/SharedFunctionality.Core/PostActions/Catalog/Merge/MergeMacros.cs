// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Templates.Core.PostActions.Catalog.Merge
{
    public static class MergeMacros
    {
        internal const string MacroBeforeMode = "^^";
        internal const string MacroStartGroup = "{[{";
        internal const string MacroEndGroup = "}]}";

        internal const string MacroStartDocumentation = "{**";
        internal const string MacroEndDocumentation = "**}";

        internal const string MacroStartDelete = "{--{";
        internal const string MacroEndDelete = "}--}";

        internal const string MacroStartOptionalContext = "{??{";
        internal const string MacroEndOptionalContext = "}??}";

        internal static string[] Macros => new string[] { MacroBeforeMode, MacroStartGroup, MacroEndGroup, MacroStartDocumentation, MacroEndDocumentation, MacroStartDelete, MacroEndDelete, MacroStartOptionalContext, MacroEndOptionalContext };
    }
}
