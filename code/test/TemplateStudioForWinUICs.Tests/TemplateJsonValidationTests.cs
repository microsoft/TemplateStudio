// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Microsoft.Templates.Test;
using TemplateValidator;
using Xunit;

namespace TemplateStudioForWinUICs.Tests
{
    [Trait("Group", "TS4WinUICs")]
    [Trait("Group", "MinimumWinUICs")]
    public partial class TemplateJsonValidationTests : BaseTemplateJsonValidationTests
    {
        [Theory]
        [MemberData(nameof(TemplateJsonValidationTests.GetAllRelativeTemplateJsonFiles))]
        public override async Task VerifyAllTemplateFilesAsync(string filePath)
        {
            var result = await TemplateJsonVerifier.VerifyTemplatePathAsync(filePath);

            Assert.True(result.Success, $"{filePath}: " + string.Join(Environment.NewLine, result.Messages));
        }
    }
}
