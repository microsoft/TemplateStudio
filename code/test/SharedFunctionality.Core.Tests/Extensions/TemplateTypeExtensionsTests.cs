// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Templates.Core.Extensions;
using Xunit;

namespace Microsoft.Templates.Core.Test.Extensions
{
    [Trait("Group", "Minimum")]
    public class TemplateTypeExtensionsTests
    {
        private const int PROJECT = 0;
        private const int PAGE = 1;
        private const int FEATURE = 2;
        private const int SERVICE = 3;
        private const int TESTING = 4;
        private const int COMPOSITION = 5;
        private const int UNSPECIFIED = 6;

        [Theory]
        [InlineData(PAGE)]
        [InlineData(FEATURE)]
        [InlineData(SERVICE)]
        [InlineData(TESTING)]
        public void IsItemTemplate_ShouldBeTrue(int templateTypeId)
        {
            TemplateType templateType = (TemplateType)templateTypeId;
            var actual = templateType.IsItemTemplate();

            Assert.True(actual);
        }

        [Theory]
        [InlineData(PROJECT)]
        [InlineData(COMPOSITION)]
        [InlineData(UNSPECIFIED)]
        public void IsItemTemplate_ShouldBeFalse(int templateTypeId)
        {
            TemplateType templateType = (TemplateType)templateTypeId;
            var actual = templateType.IsItemTemplate();

            Assert.False(actual);
        }

        private const int ADDPAGE = 1;
        private const int ADDFEATURE = 2;
        private const int ADDSERVICE = 3;
        private const int ADDTESTING = 4;

        [Theory]
        [InlineData(PAGE)]
        [InlineData(FEATURE)]
        [InlineData(SERVICE)]
        [InlineData(TESTING)]
        public void GetWizardType_ShouldNotBeNull(int templateTypeId)
        {
            TemplateType templateType = (TemplateType)templateTypeId;
            var actual = templateType.GetWizardType();

            Assert.NotNull(actual);
        }

        [Theory]
        [InlineData(PAGE, ADDPAGE)]
        [InlineData(FEATURE, ADDFEATURE)]
        [InlineData(SERVICE, ADDSERVICE)]
        [InlineData(TESTING, ADDTESTING)]
        public void GetWizardType_IsItemTemplate_ShouldMatchAValidWizardType(int templateTypeId, int? wizardTypeId)
        {
            TemplateType templateType = (TemplateType)templateTypeId;
            var actual = templateType.GetWizardType();

            Assert.Equal(wizardTypeId, (int?)actual.Value);
        }

        [Theory]
        [InlineData(PROJECT)]
        [InlineData(COMPOSITION)]
        [InlineData(UNSPECIFIED)]
        public void GetWizardType_NotItemTemplate_ShouldReturnNull(int templateTypeId)
        {
            TemplateType templateType = (TemplateType)templateTypeId;
            var actual = templateType.GetWizardType();

            Assert.Null(actual);
        }
    }
}
