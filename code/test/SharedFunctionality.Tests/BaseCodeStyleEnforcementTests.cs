// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace Microsoft.Templates.Test
{
    public abstract class BaseCodeStyleEnforcementTests
    {
        // This is the relative path from where the test assembly will run from
        public abstract string TemplatesRoot();

        [Fact]
        public void EnsureCSharpCodeDoesNotUseThisDot()
        {
            var filesNeedsToUseThis = new List<string>()
            {
                "ContentGridViewDetailPage.xaml.cs",
                @"Pg\CGrid\Param_ProjectName\Views\Param_ItemNameDetailPage.xaml.cs",
            };
            var result = CodeIsNotUsed("this.", ".cs", filesNeedsToUseThis);

            Assert.True(result.Item1, result.Item2);
        }

        // Some of the merge functionality includes whitespace in string comparisons.
        // Ensuring all whitespace is spaces avoids issues where strings differ due to different whitespace (which can be hard to spot)
        public void EnsureTabsNotUsed(string fileExtension)
        {
            var filesWithTabs = new List<string>() { };

            var result = CodeIsNotUsed('\t'.ToString(), fileExtension, filesWithTabs);

            Assert.True(result.Item1, result.Item2);
        }

        public Tuple<bool, string> CodeIsNotUsed(string textThatShouldNotBeInTheFile, string fileExtension, IEnumerable<string> filesToExclude = null)
        {
            foreach (var file in GetFiles(TemplatesRoot(), fileExtension).Where(f => filesToExclude == null || !filesToExclude.Any(fe => f.Contains(fe))))
            {
                if (File.ReadAllText(file).Contains(textThatShouldNotBeInTheFile))
                {
                    // Throw an assertion failure here and stop checking other files.
                    // We don't need to check every file if at least one fails as this should just be a final verification.
                    return new Tuple<bool, string>(false, $"The file '{file}' contains '{textThatShouldNotBeInTheFile}' but based on our style guidelines it shouldn't.");
                }
            }

            return new Tuple<bool, string>(true, string.Empty);
        }

        public Tuple<bool, string> CodeDoesNotMatchRegex(string codeRegexThatShouldNotBeInTheFile, string fileExtension, IEnumerable<string> filesToExclude = null)
        {
            foreach (var file in GetFiles(TemplatesRoot(), fileExtension).Where(f => filesToExclude == null || !filesToExclude.Any(fe => f.Contains(fe))))
            {
                if (Regex.IsMatch(File.ReadAllText(file), codeRegexThatShouldNotBeInTheFile))
                {
                    // Throw an assertion failure here and stop checking other files.
                    // We don't need to check every file if at least one fails as this should just be a final verification.
                    return new Tuple<bool, string>(false, $"The file '{file}' contains code that matches the regex '{codeRegexThatShouldNotBeInTheFile}' but based on our style guidelines it shouldn't.");
                }
            }

            return new Tuple<bool, string>(true, string.Empty);
        }

        public IEnumerable<string> GetFiles(string directory, string extension = ".*")
        {
            foreach (var dir in Directory.GetDirectories(directory))
            {
                foreach (var file in Directory.GetFiles(dir, $"*{extension}"))
                {
                    yield return file;
                }

                foreach (var file in GetFiles(dir, extension))
                {
                    yield return file;
                }
            }
        }

        public IEnumerable<string> GetFilesWithExtension(string extension)
        {
            foreach (var file in GetFiles(TemplatesRoot(), extension))
            {
                yield return file;
            }
        }
    }
}
