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

using Xunit;

namespace Microsoft.Templates.Test
{
    public class FileEncodingTests
    {
        [Fact]
        public void EnsureAllTemplateFilesAreEncodedCorrectly()
        {
            // This is the relative path from where the test assembly will run from
            const string templatesRoot = "../../../../../Templates";

            var interestedExtensions = new[]
            {
                ".appxmanifest",
                ".cs",
                ".csproj",
                ".json",
                ".md",
                ".resw",
                ".vb",
                ".vbproj",
                ".xaml",
                ".xml",
            };

            foreach (var file in GetFiles(templatesRoot))
            {
                if (interestedExtensions.Contains(Path.GetExtension(file)))
                {
                    if (!IsEncodedCodedCorrectly(file))
                    {
                        // Throw an assertion failure here and stop checking other files.
                        // We don't need to check every file if at least one fails as all can be fixed at once.
                        Assert.True(false, $"At least one file ({file}) is not encoded correctly. Ensure all template files are encoded correctly with the script at '_utils/Set-All-Text-File-Template-Encodings.ps1'");
                    }
                }
            }

            // If we get here all files were encoded correctly
            Assert.True(true);
        }

        private bool IsEncodedCodedCorrectly(string filePath)
        {
            var buffer = new byte[3];

            using (var fs = new FileStream(filePath, FileMode.Open))
            {
                fs.Read(buffer, 0, 3);
                fs.Close();
            }

            // UTF-8 with Byte Order Mark (BOM aka Signature) starts EF BB BF
            return buffer[0] == 0xef && buffer[1] == 0xbb && buffer[2] == 0xbf;
        }

        private IEnumerable<string> GetFiles(string directory)
        {
            foreach (var dir in Directory.GetDirectories(directory))
            {
                foreach (var file in Directory.GetFiles(dir))
                {
                    yield return file;
                }

                foreach (var file in GetFiles(dir))
                {
                    yield return file;
                }
            }
        }
    }
}
