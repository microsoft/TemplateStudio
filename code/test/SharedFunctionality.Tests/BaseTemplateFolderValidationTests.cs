// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Microsoft.Templates.Test
{
    public abstract class BaseTemplateFolderValidationTests
    {
        public abstract void VerifyTemplateFolderContents();

        public abstract void EnsureTemplateFilesDoNotExceedPathLength();

        public void EnsureTemplateFilesDoNotExceedPathLengthInternal(string templatesRoot)
        {
            var exceedingTemplates = new List<string>();

            // Update this to appropriate (even smaller--as small as practical) value
            var maxLength = 120;

            foreach (var file in GetFiles(templatesRoot))
            {
                var path = Path.GetFullPath(file);
                var templateRoot = Path.GetFullPath(templatesRoot);

                var relativePath = path.Replace(templateRoot, string.Empty);


                if (relativePath.Length > maxLength)
                {
                    exceedingTemplates.Add($"{relativePath} ({relativePath.Length})");
                }
            }

            Assert.True(exceedingTemplates.Count == 0, $"Following relative template paths exceed {maxLength} chars:{Environment.NewLine}{string.Join(Environment.NewLine, exceedingTemplates.ToArray())}");
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
