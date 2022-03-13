// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Xunit;

namespace Microsoft.Templates.Core.Test.Extensions
{
    [Trait("Group", "Minimum")]
    public class DateTimeExtensionsTests
    {
        private readonly DateTime date;

        public DateTimeExtensionsTests()
        {
            date = new DateTime(2000, 1, 2, 3, 4, 5, 6);
        }

        [Fact]
        public void FormatAsDateForFilePath_ShouldReturnCorrectStringDate()
        {
            var factData = date;

            var expected = "20000102";

            var result = factData.FormatAsDateForFilePath();

            Assert.Equal(expected: expected, actual: result);
        }

        [Fact]
        public void FormatAsFullDateTime_ShouldReturnCorrectStringDate()
        {
            var factData = date;

            var expected = "2000-01-02 03:04:05.006";

            var result = factData.FormatAsFullDateTime();

            Assert.Equal(result, expected);
        }

        [Fact]
        public void FormatAsTime_ShouldReturnCorrectStringDate()
        {
            var factData = date;

            var expected = "03:04:05.006";

            var result = factData.FormatAsTime();

            Assert.Equal(result, expected);
        }

        [Fact]
        public void FormatAsShortDateTime_ShouldReturnCorrectStringDate()
        {
            var factData = date;

            var expected = "20000102_030405";

            var result = factData.FormatAsShortDateTime();

            Assert.Equal(result, expected);
        }

        [Fact]
        public void FormatAsDateHoursMinutes_ShouldReturnCorrectStringDate()
        {
            var factData = date;

            var expected = "02030405";

            var result = factData.FormatAsDateHoursMinutes();

            Assert.Equal(result, expected);
        }
    }
}
