// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
