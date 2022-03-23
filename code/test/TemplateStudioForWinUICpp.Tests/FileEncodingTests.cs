// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Templates.Test;
using Xunit;

namespace TemplateStudioForWinUICpp.Tests
{
    [Trait("Group", "TS4WinUICpp")]
    [Trait("Group", "MinimumWinUICpp")]
    public class FileEncodingTests : BaseFileEncodingTests
    {
        [Fact]
        public override void EnsureAllTemplateFilesAreEncodedCorrectly()
        {
            EnsureEncodingsInternal(TestConstants.Ts4WinuiCppRelativeTemplatesRoot);
        }
    }
}
