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
