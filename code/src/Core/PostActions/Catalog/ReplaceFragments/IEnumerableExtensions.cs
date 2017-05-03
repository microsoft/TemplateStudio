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

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Microsoft.Templates.Core.PostActions.Catalog.ReplaceFragments
{
    public static class ListStringExtensions
    {
        private const string FragmentIndicator = "// FRAGMENT ";

        public static bool ReplaceFragments(this List<string> classContent)
        {
            if (classContent == null || !classContent.Any())
            {
                return false;
            }

            bool replacementsMade = false;

            for (var index = classContent.Count - 1; index > 0; index--)
            {
                var line = classContent[index];

                if (line.StartsWith(FragmentIndicator))
                {
                    var path = line.Substring(FragmentIndicator.Length);

                    path = Path.Combine(Gen.GenContext.ToolBox.Repo.CurrentContentFolder, path);

                    if (File.Exists(path))
                    {
                        var fileContents = File.ReadAllLines(path);

                        classContent.RemoveAt(index);

                        classContent.InsertRange(index, fileContents);

                        replacementsMade = true;
                    }
                }
            }

            return replacementsMade;
        }
    }
}
