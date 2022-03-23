// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Microsoft.Templates.Core.Composition;
using Xunit;

namespace Microsoft.Templates.Core.Test.Extensions
{
    [Trait("Group", "Minimum")]
    public class DictionaryExtensionsTests
    {
        private const int MAX = 100;
        private readonly Random random;
        private readonly Dictionary<string, string> dictionaryOfStrings;
        private readonly Dictionary<string, QueryableProperty> dictionaryOfQueryable;

        public DictionaryExtensionsTests()
        {
            random = new Random();
            dictionaryOfStrings = new Dictionary<string, string>();
            dictionaryOfQueryable = new Dictionary<string, QueryableProperty>();
        }

        private void SetUpStringData(int items)
        {
            Assert.True(items > 0);

            for (var i = 0; i < items; i++)
            {
                dictionaryOfStrings.Add($"key{i + 1}", $"value{i + 1}");
            }
        }

        [Fact]
        public void SafeGet_DictionaryOfStrings_NotFound_ShouldReturnDefault()
        {
            string expected = null;

            var actual = dictionaryOfStrings.SafeGet("test");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SafeGet_DictionaryOfStrings_NotFound_ShouldReturnConfiguredDefault()
        {
            string expected = "default";

            var actual = dictionaryOfStrings.SafeGet("test", "default");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SafeGet_DictionaryOfStrings_Found_ShouldReturnItem()
        {
            var randomItemsNumber = random.Next(MAX) + 1;
            SetUpStringData(randomItemsNumber);

            var expected = $"value{randomItemsNumber}";

            var actual = dictionaryOfStrings.SafeGet($"key{randomItemsNumber}");

            Assert.Equal(expected, actual);
        }

        private void SetUpQueryableData(int items)
        {
            Assert.True(items > 0);

            for (var i = 0; i < items; i++)
            {
                dictionaryOfQueryable.Add($"key{i + 1}", new QueryableProperty($"name{i + 1}", $"value{i + 1}"));
            }
        }

        [Fact]
        public void SafeGet_DictionaryOfQueryable_NotFound_ShouldReturnDefault()
        {
            QueryableProperty expected = null;

            var actual = dictionaryOfQueryable.SafeGet("test");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SafeGet_DictionaryOfQueryable_NotFound_ShouldReturnConfiguredDefault()
        {
            QueryableProperty expected = QueryableProperty.Empty;

            var actual = dictionaryOfQueryable.SafeGet("test", QueryableProperty.Empty);

            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Value, actual.Value);
        }

        [Fact]
        public void SafeGet_DictionaryOfQueryable_Found_ShouldReturnItem()
        {
            var randomItemsNumber = random.Next(MAX) + 1;
            SetUpQueryableData(randomItemsNumber);

            var expected = new QueryableProperty($"name{randomItemsNumber}", $"value{randomItemsNumber}");

            var actual = dictionaryOfQueryable.SafeGet($"key{randomItemsNumber}");

            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Value, actual.Value);
        }
    }
}
