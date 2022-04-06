// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Templates.Test;
using Xunit;

namespace TemplateStudioForUwp.Tests
{
    [Trait("Group", "MinimumUWP")]
    public class ProjectFileTests : BaseProjectFileTests
    {
        [Fact]
        public override void CheckTemplateReferences()
        {
            CheckTemplateReferencesInternal(TestConstants.Ts4UwpProjFile);
        }
    }
}
