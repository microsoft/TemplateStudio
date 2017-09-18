// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;

using Xunit;

namespace Microsoft.Templates.Test
{
    [Collection("StyleCopCollection")]
    [Trait("Type", "CodeStyle")]
    [Trait("ExecutionSet", "Minimum")]
    public class CodeStyleEnforcementTests
    {
        [Fact]
        public void EnsureCSharpCodeDoesNotUseThis()
        {
            var result = CodeIsNotUsed("this.", ".cs");

            Assert.True(result.Item1, result.Item2);
        }

        [Fact]
        public void EnsureCSharpCodeDoesNotUseTabs()
        {
            var result = CodeIsNotUsed('\t'.ToString(), ".cs");

            Assert.True(result.Item1, result.Item2);
        }

        [Fact]
        public void EnsureVisualbasicCodeDoesNotUseTabs()
        {
            var result = CodeIsNotUsed('\t'.ToString(), ".vb");

            Assert.True(result.Item1, result.Item2);
        }

        private Tuple<bool, string> CodeIsNotUsed(string textThatShouldNotBeinTheFile, string fileExtension)
        {
            // This is the relative path from where the test assembly will run from
            const string templatesRoot = "..\\..\\..\\..\\..\\Templates";

            foreach (var file in GetFiles(templatesRoot, fileExtension))
            {
                var allText = File.ReadAllText(file);

                if (allText.Contains(textThatShouldNotBeinTheFile)
                 && !IsValidException(textThatShouldNotBeinTheFile, file, allText))
                {
                    // Throw an assertion failure here and stop checking other files.
                    // We don't need to check every file if at least one fails as this should just be a final verification.
                    return new Tuple<bool, string>(false, $"The file '{file}' contains '{textThatShouldNotBeinTheFile}' but based on our style guidelines it shouldn't.");
                }
            }

            return new Tuple<bool, string>(true, string.Empty);
        }

        private bool IsValidException(string textThatShouldNotBeinTheFile, string fileName, string fileContents)
        {
            switch (textThatShouldNotBeinTheFile)
            {
                case "this.":
                    // The CaliburnMicro templates include a Set method that is an extension method on PropertyChangedBase.
                    // Calling it requires referencing `this` and so is a valid exception
                    if (fileName.Contains("CaliburnMicro"))
                    {
                        var fileContentsMinusValidExceptions = fileContents.Replace("set { this.Set(", "XXXXX");

                        return !fileContentsMinusValidExceptions.Contains(textThatShouldNotBeinTheFile);
                    }

                    break;
                default:
                    break;
            }

            return false;
        }

        private IEnumerable<string> GetFiles(string directory, string extension = ".*")
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
    }
}
