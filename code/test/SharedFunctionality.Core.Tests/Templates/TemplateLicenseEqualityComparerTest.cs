// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;

namespace Microsoft.Templates.Core.Test.Templates
{
    [Trait("Group", "Minimum")]
    public class TemplateLicenseEqualityComparerTest
    {
        [Fact]
        public void Compare_EqualNull()
        {
            TemplateLicense license1 = null;
            TemplateLicense license2 = null;

            var comparer = new TemplateLicenseEqualityComparer();
            var result = comparer.Equals(license1, license2);

            Assert.True(result);
        }

        [Fact]
        public void Compare_EqualLicense()
        {
            TemplateLicense license1 = new TemplateLicense() { Text = "TestText", Url = "TestUrl" };
            TemplateLicense license2 = new TemplateLicense() { Text = "TestText", Url = "TestUrl" };

            var comparer = new TemplateLicenseEqualityComparer();
            var result = comparer.Equals(license1, license2);

            Assert.True(result);
        }

        [Fact]
        public void Compare_EqualLicenseDifferentText()
        {
            TemplateLicense license1 = new TemplateLicense() { Text = "TestText", Url = "TestUrl" };
            TemplateLicense license2 = new TemplateLicense() { Text = "OtherText", Url = "TestUrl" };

            var comparer = new TemplateLicenseEqualityComparer();
            var result = comparer.Equals(license1, license2);

            Assert.True(result);
        }

        [Fact]
        public void Compare_DifferentNull()
        {
            TemplateLicense license1 = null;
            TemplateLicense license2 = new TemplateLicense() { Text = "TestText", Url = "TestUrl" };

            var comparer = new TemplateLicenseEqualityComparer();
            var result = comparer.Equals(license1, license2);

            Assert.False(result);
        }

        [Fact]
        public void Compare_DifferentLicenseText()
        {
            TemplateLicense license1 = new TemplateLicense() { Text = "TestText", Url = "TestUrl" };
            TemplateLicense license2 = new TemplateLicense() { Text = "OtherText", Url = "OtherUrl" };

            var comparer = new TemplateLicenseEqualityComparer();
            var result = comparer.Equals(license1, license2);

            Assert.False(result);
        }
    }
}
