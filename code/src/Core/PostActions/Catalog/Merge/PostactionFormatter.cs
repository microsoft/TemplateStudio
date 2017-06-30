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
using Microsoft.Templates.Core.Resources;

namespace Microsoft.Templates.Core.PostActions.Catalog.Merge
{
    public static class PostActionFormatter
    {
        public static string AsUserFriendlyPostAction(this string postactionCode)
        {
            var output = postactionCode
                            .Replace(IEnumerableExtensions.MacroBeforeMode, string.Empty)
                            .Replace(IEnumerableExtensions.MacroStartGroup, StringRes.UserFriendlyPostActionMacroStartGroup)
                            .Replace(IEnumerableExtensions.MarcoEndGroup, StringRes.UserFriendlyPostActionMacroEndGroup);

            var cleanRemovals = output.Split(new[] { Environment.NewLine }, StringSplitOptions.None).RemoveRemovals();
            output = string.Join(Environment.NewLine, cleanRemovals);
            return output;
        }
    }
}
