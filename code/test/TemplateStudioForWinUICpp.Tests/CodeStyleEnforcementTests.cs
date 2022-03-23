// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Templates.Test;
using Xunit;

namespace TemplateStudioForWinUICpp.Tests
{
    [Trait("Group", "Minimum")]
    public class CodeStyleEnforcementTests : BaseCodeStyleEnforcementTests
    {
        public override string TemplatesRoot() => TestConstants.Ts4WinuiCppRelativeTemplatesRoot ;

        [Fact]
        public void EnsureCppTemplatesDoNotUseTabsAsWhitespace()
        {
            EnsureTabsNotUsed("*.cpp");
        }
    }
}
