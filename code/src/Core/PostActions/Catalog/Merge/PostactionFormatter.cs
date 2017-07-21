// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Templates.Core.Resources;

namespace Microsoft.Templates.Core.PostActions.Catalog.Merge
{
    public static class PostActionFormatter
    {
        private const string UserFriendlyPostActionMacroBeforeMode = "Include the following block at the end of the containing block.";
        private const string UserFriendlyPostActionMacroStartGroup = "Block to be included";
        private const string UserFriendlyPostActionMacroEndGroup = "End of block";
        private const string UserFriendlyPostActionMacroStartDocumentation = "***";
        private const string UserFriendlyPostActionMacroEndDocumentation = "***";

        public static string AsUserFriendlyPostAction(this string postactionCode)
        {
            var output = postactionCode
                            .Replace(IEnumerableExtensions.MacroBeforeMode, UserFriendlyPostActionMacroBeforeMode)
                            .Replace(IEnumerableExtensions.MacroStartDocumentation, UserFriendlyPostActionMacroStartDocumentation)
                            .Replace(IEnumerableExtensions.MacroEndDocumentation, UserFriendlyPostActionMacroEndDocumentation)
                            .Replace(IEnumerableExtensions.MacroStartGroup, UserFriendlyPostActionMacroStartGroup)
                            .Replace(IEnumerableExtensions.MarcoEndGroup, UserFriendlyPostActionMacroEndGroup);

            var cleanRemovals = output.Split(new[] { Environment.NewLine }, StringSplitOptions.None).RemoveRemovals();
            output = string.Join(Environment.NewLine, cleanRemovals);
            return output;
        }
    }
}
