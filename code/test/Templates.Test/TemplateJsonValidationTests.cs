// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TemplateValidator;
using Xunit;

namespace Microsoft.Templates.Test
{
    [Trait("Type", "TemplateValidation")]
    [Trait("ExecutionSet", "Minimum")]
    [Trait("ExecutionSet", "TemplateValidation")]
    public class TemplateJsonValidationTests
    {
        public static IEnumerable<object[]> GetAllTemplateJsonFiles()
        {
            // This is the relative path from where the test assembly will run from
            const string templatesRoot = "../../../../../Templates";

            // The following excludes the catalog and project folders, but they only contain a single template file each
            var foldersOfInterest = new[] { "Uwp/_comp", "Uwp/Features", "Uwp/Pages" };

            foreach (var folder in foldersOfInterest)
            {
                foreach (var file in new DirectoryInfo(Path.Combine(templatesRoot, folder)).GetFiles("template.json", SearchOption.AllDirectories))
                {
                    yield return new object[] { file.FullName };
                }
            }
        }

        private static IEnumerable<string> GetFiles(string directory)
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

        [Theory]
        [MemberData(nameof(GetAllTemplateJsonFiles))]
        public async Task VerifyAllTemplateFilesAsync(string filePath)
        {
            var result = await TemplateJsonVerifier.VerifyTemplatePathAsync(filePath);

            Assert.True(result.Success, $"{filePath}: " + string.Join(Environment.NewLine, result.Messages));
        }
    }
}
