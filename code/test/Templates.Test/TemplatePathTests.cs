// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;

using Xunit;

namespace Microsoft.Templates.Test
{
    [Trait("ExecutionSet", "Minimum")]
    [Trait("ExecutionSet", "TemplateValidation")]
    [Trait("ExecutionSet", "_CIBuild")]
    [Trait("ExecutionSet", "_Full")]
    public class TemplatePathTests
    {
        [Fact]
        public void EnsureTemplateFilesDoNotExceedPathLength()
        {
            // This is the relative path from where the test assembly will run from
            const string templatesRoot = "../../../../../Templates";

            var exceedingTemplates = new List<string>();
            foreach (var file in GetFiles(templatesRoot))
            {
                var path = Path.GetFullPath(file);
                var templateRoot = Path.GetFullPath(templatesRoot);

                var relativePath = path.Replace(templateRoot, string.Empty);

                if (relativePath.Length > 130)
                {
                    exceedingTemplates.Add(relativePath);
                }     
            }

            Assert.True(exceedingTemplates.Count == 0, $"Following templates exceed 130 chars:  {string.Join(Environment.NewLine, exceedingTemplates.ToArray())}");
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
