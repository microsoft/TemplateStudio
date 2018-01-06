// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace Microsoft.Templates.Test
{
    [Collection("StyleCopCollection")]
    [Trait("Type", "CodeStyle")]
    [Trait("ExecutionSet", "Minimum")]
    [Trait("ExecutionSet", "TemplateValidation")]
    public class CodeStyleEnforcementTests
    {
        // This is the relative path from where the test assembly will run from
        private const string TemplatesRoot = "..\\..\\..\\..\\..\\Templates";

        [Fact]
        public void EnsureCSharpCodeDoesNotUseThis()
        {
            var result = CodeIsNotUsed("this.", ".cs");

            Assert.True(result.Item1, result.Item2);
        }

        [Fact]
        public void EnsureTemplatesDoNotUseTabsInWhitespace()
        {
            // Some of the merge functionality includes whitespace in string comparisons.
            // Ensuring all whitespace is spaces avoids issues where strings differ due to different whitespace (which can be hard to spot)
            void EnsureTabsNotUsed(string fileExtension)
            {
                var result = CodeIsNotUsed('\t'.ToString(), fileExtension);

                Assert.True(result.Item1, result.Item2);
            }

            EnsureTabsNotUsed("*.cs");
            EnsureTabsNotUsed("*.vb");
        }

        [Fact]
        public void EnsureCodeDoesNotUseOldTodoCommentIdentifier()
        {
            void EnsureUwpTemplatesNotUsed(string fileExtension)
            {
                var result = CodeIsNotUsed("UWPTemplates", fileExtension);

                Assert.True(result.Item1, result.Item2);
            }

            EnsureUwpTemplatesNotUsed("*.cs");
            EnsureUwpTemplatesNotUsed("*.vb");
        }

        [Fact]
        public void EnsureVisualBasicCodeDoesNotIncludeCommonPortingIssues()
        {
            var foundErrors = new List<string>();

            // Build tests will fail if these are included but this test is quicker than building everything
            void CheckStringNotIncluded(string toSearchFor)
            {
                var result = CodeIsNotUsed(toSearchFor, ".vb");

                if (!result.Item1)
                {
                    foundErrors.Add(result.Item2);
                }
            }

            void IfLineIncludes(string ifIncludes, string itMustAlsoInclude, params string[] unlessItContains)
            {
                foreach (var file in GetFiles(TemplatesRoot, ".vb"))
                {
                    foreach (var line in File.ReadAllLines(file))
                    {
                        if (line.Contains(ifIncludes) && !line.Contains(itMustAlsoInclude))
                        {
                            var foundException = false;

                            if (unlessItContains != null)
                            {
                                foreach (var unless in unlessItContains)
                                {
                                    if (line.Contains(unless))
                                    {
                                        foundException = true;
                                        break;
                                    }
                                }
                            }

                            if (!foundException)
                            {
                                foundErrors.Add($"The file '{file}' contains '{ifIncludes}' but doesn't also include '{itMustAlsoInclude}'.");
                            }
                        }
                    }
                }
            }

            CheckStringNotIncluded("Namespace Param_RootNamespace."); // Root namespace is included by default in VB
            CheckStringNotIncluded("Namespace Param_ItemNamespace."); // Root namespace is included by default in VB
            CheckStringNotIncluded(";");
            CheckStringNotIncluded("var "); // May be in commented our code included in template as an example
            CheckStringNotIncluded("Var "); // May be in commented our code included in template as an example
            CheckStringNotIncluded("Key ."); // Output by converter as part of object initializers
            CheckStringNotIncluded("yield Return"); // Return not needed but converter includes it
            CheckStringNotIncluded("yield return"); // Return not needed but converter includes it
            CheckStringNotIncluded("wts__"); // temporary placeholder used during conversion
            CheckStringNotIncluded("'''/");

            IfLineIncludes(" As Task", itMustAlsoInclude: " Async ", unlessItContains: new[] { " MustOverride ", "Function RunAsync(", "Function RunAsyncInternal(", " FireAndForget(" });

            IfLineIncludes("\"{", itMustAlsoInclude: "$");

            Assert.True(foundErrors.Count == 0, string.Join(Environment.NewLine, foundErrors));
        }

        [Fact]
        public void EnsureVisualBasicCodeDoesNotUseOnErrorGoto()
        {
            var result = CodeIsNotUsed("On Error Goto", "*.vb");

            Assert.True(result.Item1, result.Item2);
        }

        [Fact]
        public void EnsureVisualBasicCodeDoesNotContainMultipleConsecutiveBlankLines()
        {
            var result = CodeIsNotUsed($"{Environment.NewLine}{Environment.NewLine}{Environment.NewLine}", "*.vb");

            Assert.True(result.Item1, result.Item2);
        }

        [Fact]
        public void EnsureVisualBasicCodeDoesNotIndicateParamsPassedByVal()
        {
            var result = CodeIsNotUsed("ByVal", "*.vb");

            Assert.True(result.Item1, result.Item2);
        }

        // Disabled as failing on AppVeyor for some files with no obvious reason
        ////[Fact]
        public void EnsureVisualBasicFilesEndWithSingleBlankLine()
        {
            var errorFiles = new List<string>();

            foreach (var file in GetFiles(TemplatesRoot, "*.vb"))
            {
                var text = File.ReadAllText(file, Encoding.UTF8);

                if (!text.EndsWith(Environment.NewLine, StringComparison.InvariantCulture)
                 || text.EndsWith(Environment.NewLine + Environment.NewLine, StringComparison.InvariantCulture))
                {
                    errorFiles.Add(file);
                }
            }

            Assert.True(errorFiles.Count == 0, $"The following files don't end with a single NewLine{Environment.NewLine}{string.Join(Environment.NewLine, errorFiles)}");
        }

        private Tuple<bool, string> CodeIsNotUsed(string textThatShouldNotBeinTheFile, string fileExtension)
        {
            foreach (var file in GetFiles(TemplatesRoot, fileExtension))
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
