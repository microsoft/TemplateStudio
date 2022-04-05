// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Templates.Test;
using Xunit;

namespace TemplateStudioForUwp.Tests
{
    [Trait("Group", "MinimumUWP")]
    public class CodeStyleEnforcementTests : BaseCodeStyleEnforcementTests
    {
        public override string TemplatesRoot() => TestConstants.Ts4UwpRelativeTemplatesRoot;

        [Fact]
        public void EnsureCsTemplatesDoNotUseTabsAsWhitespace()
        {
            EnsureTabsNotUsed("*.cs");
        }

        [Fact]
        public void EnsureVbTemplatesDoNotUseTabsAsWhitespace()
        {
            EnsureTabsNotUsed("*.vb");
        }

        [Fact]
        public void EnsureUwpTemplatesDefineNamespacesCorrectly()
        {
            var result = new List<string>();

            void EnsureDoNotUse(string shouldNotUse, string fileExtension)
            {
                var (success, failMessage) = CodeIsNotUsed(shouldNotUse, fileExtension);

                if (!success)
                {
                    result.Add(failMessage + " It should use 'Param_RootNamespace' instead.");
                }
            }

            // The placeholder "Param_RootNamespace" should be used instead, to ensure that all namespaces are created equally
            EnsureDoNotUse("namespace wts.DefaultProject", "*.cs");
            EnsureDoNotUse("namespace wts.DefaultProject", "*.vb");
            EnsureDoNotUse("namespace Param_ProjectName", "*.cs");
            EnsureDoNotUse("namespace Param_ProjectName", "*.vb");
            EnsureDoNotUse("namespace Param_ItemNamespace", "*.cs");
            EnsureDoNotUse("namespace Param_ItemNamespace", "*.vb");
            EnsureDoNotUse("namespace wts.ItemName", "*.cs");
            EnsureDoNotUse("namespace wts.ItemName", "*.vb");
            EnsureDoNotUse("using wts.ItemName.", "*.cs");
            EnsureDoNotUse("Imports wts.ItemName.", "*.vb");
            EnsureDoNotUse("using wts.DefaultProject", "*.cs");
            EnsureDoNotUse("Imports wts.DefaultProject", "*.vb");
            EnsureDoNotUse("using Param_ItemNamespace", "*.cs");
            EnsureDoNotUse("Imports Param_ItemNamespace", "*.vb");
            EnsureDoNotUse("using Param_ProjectName", "*.cs");
            EnsureDoNotUse("Imports Param_ProjectName", "*.vb");
            EnsureDoNotUse("x:Class=\"wts.DefaultProject", "*.xaml");
            EnsureDoNotUse("using:wts.ItemName", "*.xaml");

            Assert.True(!result.Any(), string.Join(Environment.NewLine, result));
        }

        [Fact]
        public void EnsureUwpCodeDoesNotUseOldTodoCommentIdentifier()
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
        public void EnsureUwpCodeDoesNotUseInvalidTodoCommentIdentifier()
        {
            void EnsureWTSTODONotUsed(string fileExtension)
            {
                var result = CodeIsNotUsed("WTS TODO", fileExtension);

                Assert.True(result.Item1, result.Item2);
            }

            EnsureWTSTODONotUsed("*.cs");
            EnsureWTSTODONotUsed("*.vb");
        }

        [Fact]
        public void EnsureCSharpCodeDoesNotUseLocalizedExceptionMessages()
        {
            var result = CodeDoesNotMatchRegex("(throw).*(Exception\\().*(GetLocalized)", "*.cs");

            Assert.True(result.Item1, result.Item2);

        }

        [Fact]
        public void EnsureVisualBasicCodeDoesNotUseLocalizedExceptionMessages()
        {
            var result = CodeDoesNotMatchRegex("(Throw).*(Exception\\().*(GetLocalized)", "*.vb");

            Assert.True(result.Item1, result.Item2);
        }

        [Fact]
        public void EnsureVisualBasicCodeDoesNotIncludeCommonPortingIssues()
        {
            var foundErrors = new List<string>();

            // Build tests will fail if these are included but this test is quicker than building everything
            void CheckStringNotIncluded(string toSearchFor, string exception = null)
            {
                var result = CodeIsNotUsed(toSearchFor, ".vb");

                if (!result.Item1)
                {
                    if (string.IsNullOrEmpty(exception) || !result.Item2.Contains(exception))
                    {
                        foundErrors.Add(result.Item2);
                    }
                }
            }

            void IfLineIncludes(string ifIncludes, string itMustAlsoInclude, params string[] unlessItContains)
            {
                foreach (var file in GetFilesWithExtension("*.vb"))
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
            CheckStringNotIncluded(";", exception: "SqlServerDataService.vb");
            CheckStringNotIncluded("var "); // May be in commented our code included in template as an example
            CheckStringNotIncluded("Var "); // May be in commented our code included in template as an example
            CheckStringNotIncluded("Key ."); // Output by converter as part of object initializers
            CheckStringNotIncluded("yield Return"); // Return not needed but converter includes it
            CheckStringNotIncluded("yield return"); // Return not needed but converter includes it
            CheckStringNotIncluded("wts__"); // temporary placeholder used during conversion
            CheckStringNotIncluded("'''/");
            CheckStringNotIncluded(" += AddressOf"); // Use AddHandler instead
            CheckStringNotIncluded(" -= AddressOf"); // Use RemoveHandler instead
            CheckStringNotIncluded("Param_Setter("); // ParamSetter should be in square brackets
            CheckStringNotIncluded("CSharpImpl"); // Output by converter
            CheckStringNotIncluded("//", exception: "App_postaction.xaml.vb"); // C# comment (exclusion is for URI in comments)
            CheckStringNotIncluded("?.Invoke", exception: "DragDropService_postaction.vb"); //use RaiseEvent instead

            IfLineIncludes(" As Task", itMustAlsoInclude: " Async ", unlessItContains: new[] { " MustOverride ", "Function RunAsync(", "Function RunAsyncInternal(", " FireAndForget(", "OnPivotSelectedAsync", "OnPivotUnselectedAsync", "OnPivotActivatedAsync", "TaskCanceledException" });

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
        public void EnsureVisualBasicCodeDoesNotContainTrailingWhiteSpace()
        {
            var warnings = new List<string>();

            foreach (var file in GetFilesWithExtension("*.vb"))
            {
                var lineNo = 1;
                foreach (var line in File.ReadAllLines(file))
                {
                    if (line.TrimEnd() != line)
                    {
                        warnings.Add($"Trailing whitespace in '{file}' on line {lineNo}.");
                    }

                    lineNo += 1;
                }
            }

            Assert.True(!warnings.Any(), string.Join(Environment.NewLine, warnings));
        }

        [Fact]
        public void EnsureVisualBasicCodeDoesNotIndicateParamsPassedByVal()
        {
            var result = CodeIsNotUsed("ByVal", "*.vb");

            Assert.True(result.Item1, result.Item2);
        }

        [Fact]
        public void EnsureVisualBasicFilesEndWithSingleBlankLine()
#pragma warning restore xUnit1013 // Public method should be marked as test
        {
            var errorFiles = new List<string>();

            foreach (var file in GetFilesWithExtension("*.vb"))
            {
                var text = File.ReadAllText(file, Encoding.UTF8);

                if (!text.EndsWith(Environment.NewLine, StringComparison.OrdinalIgnoreCase)
                 || text.EndsWith(Environment.NewLine + Environment.NewLine, StringComparison.OrdinalIgnoreCase))
                {
                    errorFiles.Add(file);
                }
            }

            Assert.True(errorFiles.Count == 0, $"The following files don't end with a single NewLine{Environment.NewLine}{string.Join(Environment.NewLine, errorFiles)}");
        }
    }
}
