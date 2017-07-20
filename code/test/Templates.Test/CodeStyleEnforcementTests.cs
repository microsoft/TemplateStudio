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
