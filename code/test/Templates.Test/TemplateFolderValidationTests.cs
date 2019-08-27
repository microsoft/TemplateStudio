// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using TemplateValidator;
using Xunit;

namespace Microsoft.Templates.Test
{
    [Trait("ExecutionSet", "TemplateValidation")]
    [Trait("ExecutionSet", "Minimum")]
    [Trait("ExecutionSet", "_CIBuild")]
    [Trait("ExecutionSet", "_Full")]
    public class TemplateFolderValidationTests
    {
        // Perform checks based on the entirety of the templates folder
        [Fact]
        public void VerifyTemplateFolderContentsAsync()
        {
            // This is the relative path from where the test assembly will run from
            const string templatesRoot = "../../../../../Templates";

            // Warnings are hidden in this automated test as there are some we can happily ignore.
            // Warnings are intended more for the authors of new templates.
            var result = TemplateFolderVerifier.VerifyTemplateFolders(showWarnings: false, templateFolders: templatesRoot);

            Assert.True(result.Success, string.Join(Environment.NewLine, result.Messages));
        }
    }
}
