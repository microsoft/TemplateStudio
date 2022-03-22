// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Microsoft.Templates.Test;
using TemplateValidator;
using Xunit;

namespace TemplateStudioForWinUICs.Tests
{
    [Trait("Group", "TS4WinUICS")]
    [Trait("Group", "MinimumWinUICS")]
    [Trait("Group", "Minimum")]
    public class TemplateFolderValidationTests : BaseTemplateFolderValidationTests
    {
        [Fact]
        public override void VerifyTemplateFolderContents()
        {
            // Warnings are hidden in this automated test as there are some we can happily ignore.
            // Warnings are intended more for the authors of new templates.
            var result = TemplateFolderVerifier.VerifyTemplateFolders(
                showWarnings: false,
                templateFolders: new List<string>() {
                  TestConstants.Ts4WinuiCsRelativeTemplatesRoot });

            Assert.True(result.Success, string.Join(Environment.NewLine, result.Messages));
        }

        [Fact]
        public override void EnsureTemplateFilesDoNotExceedPathLength()
        {
            EnsureTemplateFilesDoNotExceedPathLengthInternal(TestConstants.Ts4WinuiCsRelativeTemplatesRoot);
        }
    }
}
