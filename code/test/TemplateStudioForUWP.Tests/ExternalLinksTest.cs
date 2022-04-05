// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using Microsoft.Templates.Test;
using Xunit;

namespace TemplateStudioForUwp.Tests
{
    [Trait("Group", "TS4UWP")]
    [Trait("Group", "ReleaseUWP")]
    public class ExternalLinksTest : BaseExternalLinksTests
    {
        [Fact]
        public override async Task LicenseLinksAreCorrectAsync()
        {
            await CheckLicenseLinksInternalAsync(TestConstants.Ts4UwpRelativeTemplatesRoot);
        }

        [Fact]
        public override async Task DescriptionLinksAreCorrectAsync()
        {
            await DescriptionLinkTestsInternalAsync(TestConstants.Ts4UwpRelativeTemplatesRoot);
        }

        [Fact]
        public override async Task CommentLinksAreCorrectAsync()
        {
            await CommentLinkTestsInternalAsync(TestConstants.Ts4UwpRelativeTemplatesRoot);
        }
    }
}
