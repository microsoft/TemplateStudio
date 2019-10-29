// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.IO;
using System.Linq;

using Xunit;

namespace Microsoft.Templates.Test
{
    [Trait("Type", "CodeStyle")]
    [Trait("ExecutionSet", "Minimum")]
    [Trait("ExecutionSet", "TemplateValidation")]
    [Trait("ExecutionSet", "_CIBuild")]
    [Trait("ExecutionSet", "_Full")]
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
                        // Non breaking space leads to wrong characters saving with bom
                        if (File.ReadAllText(file).Contains('\u00A0'))
                        {
                            Assert.True(false, $"File ({file}) contains non breaking whitespaces, please remove them before running the encoding script. '");
                        }

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

            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                fs.Read(buffer, 0, 3);
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
