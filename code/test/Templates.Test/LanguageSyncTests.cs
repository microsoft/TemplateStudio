// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;

using Xunit;

namespace Microsoft.Templates.Test
{
    [Trait("Type", "TemplateValidation")]
    [Trait("ExecutionSet", "Minimum")]
    [Trait("ExecutionSet", "TemplateValidation")]
    [Trait("ExecutionSet", "_CIBuild")]
    [Trait("ExecutionSet", "_Full")]
    public class LanguageSyncTests
    {
        [Fact]
        public void EnsureVbTemplatesUseTranslatedDescriptionsIfCsOnesDo()
        {
            // This is the relative path from where the test assembly will run from
            const string templatesRoot = "../../../../../Templates";

            // Just check Czech files because text will be translated to all languages or none.
            // Just check the description but the fix will also copy 'xx-XX.template.json'
            foreach (var file in GetFiles(templatesRoot, "cs-CZ.description.md"))
            {
                // Only interested in templates with a VB version
                if (file.Contains("._VB\\"))
                {
                    var czechFileContents = File.ReadAllText(file);
                    var defaultFileContents = File.ReadAllText(file.Replace("cs-CZ.", string.Empty));

                    if (czechFileContents == defaultFileContents)
                    {
                        var csVersionOfCzechFileContents = File.ReadAllText(file.Replace("._VB\\", "\\"));

                        if (csVersionOfCzechFileContents != czechFileContents)
                        {
                            // Can end on first failure as PS script will fix missing translations for all templates
                            Assert.True(false, $"'{file}' is not translated but it's C# equivalent is.{Environment.NewLine}Run the script at '_utils\\Synchronize-Files-Used-By-VisualBasic-Templates.ps1' to copy the translations.");
                        }
                    }
                }
            }

            // If we get here everything is ok.
            Assert.True(true);
        }

        private IEnumerable<string> GetFiles(string directory, string searchPattern)
        {
            foreach (var dir in Directory.GetDirectories(directory))
            {
                foreach (var file in Directory.GetFiles(dir, searchPattern))
                {
                    yield return file;
                }

                foreach (var file in GetFiles(dir, searchPattern))
                {
                    yield return file;
                }
            }
        }
    }
}
