// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;

using Xunit;

namespace Microsoft.Templates.Test
{
    public class CodeStyleEnforcementTests
    {
        [Fact]
        public void EnsureCSharpCodeDoesNotUseThis()
        {
            var result = CodeIsNotUsed("this.", ".cs");

            Assert.True(result.Item1, result.Item2);
        }

        private Tuple<bool, string> CodeIsNotUsed(string textThatShouldNotBeinTheFile, string fileExtension)
        {
            // This is the relative path from where the test assembly will run from
            const string templatesRoot = "..\\..\\..\\..\\..\\Templates";

            foreach (var file in GetFiles(templatesRoot, fileExtension))
            {
                if (File.ReadAllText(file).Contains(textThatShouldNotBeinTheFile))
                {
                    // Throw an assertion failure here and stop checking other files.
                    // We don't need to check every file if at least one fails as this should just be a final verification. 
                    return new Tuple<bool, string>(false, $"The file '{file}' contains '{textThatShouldNotBeinTheFile}' but based on our style guidelines it shouldn't.");
                }
            }

            return new Tuple<bool, string>(true, string.Empty);
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
