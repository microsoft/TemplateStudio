// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Templates.Core.Casing;
using Xunit;

namespace Microsoft.Templates.Core.Test
{
    [Collection("Unit Test Templates")]
    [Trait("Group", "Minimum")]
    [Trait("Type", "ProjectGeneration")]
    public class CasingExtensionsTests
    {
        [Fact]
        public void Test_TransformToKebab()
        {
            var kebabCasingService = new TextCasing() { Type = CasingType.Kebab };

            Assert.Equal("list-detail", kebabCasingService.Transform("ListDetail"));
            Assert.Equal("list-detail", kebabCasingService.Transform("List_Detail"));
            Assert.Equal("list-detail", kebabCasingService.Transform("List Detail"));
            Assert.Equal("list-detail", kebabCasingService.Transform(" List Detail "));
            Assert.Equal("list-detail", kebabCasingService.Transform("list detail"));
            Assert.Equal("list-detail-123-abc", kebabCasingService.Transform("list   -  detail 123 abc"));

            Assert.Equal("list-detail-1", kebabCasingService.Transform("ListDetail1"));
            Assert.Equal("list-ui", kebabCasingService.Transform("ListUI"));
            Assert.Equal("list-ui", kebabCasingService.Transform("List UI"));
            Assert.Equal("list-ui", kebabCasingService.Transform("List_UI"));
        }

        [Fact]
        public void Test_TransformToSnake()
        {
            var snakeCasingService = new TextCasing() { Type = CasingType.Snake };

            Assert.Equal("list_detail", snakeCasingService.Transform("ListDetail"));
            Assert.Equal("list_detail", snakeCasingService.Transform("List_Detail"));
            Assert.Equal("list_detail", snakeCasingService.Transform("List Detail"));
            Assert.Equal("list_detail", snakeCasingService.Transform(" List Detail "));
            Assert.Equal("list_detail", snakeCasingService.Transform("list detail"));
            Assert.Equal("list_detail_123_abc", snakeCasingService.Transform("list   -  detail 123 abc"));

            Assert.Equal("list_detail_1", snakeCasingService.Transform("ListDetail1"));
            Assert.Equal("list_ui", snakeCasingService.Transform("ListUI"));
            Assert.Equal("list_ui", snakeCasingService.Transform("List UI"));
            Assert.Equal("list_ui", snakeCasingService.Transform("List_UI"));
        }

        [Fact]
        public void Test_TransformToPascalCase()
        {
            var pascalCasingService = new TextCasing() { Type = CasingType.Pascal };

            Assert.Equal("ListDetail", pascalCasingService.Transform("ListDetail"));
            Assert.Equal("ListDetail", pascalCasingService.Transform("List_Detail"));
            Assert.Equal("ListDetail", pascalCasingService.Transform("List Detail"));
            Assert.Equal("ListDetail", pascalCasingService.Transform(" List Detail "));
            Assert.Equal("ListDetail1", pascalCasingService.Transform("list detail 1"));

            Assert.Equal("ListUI", pascalCasingService.Transform("ListUI"));
            Assert.Equal("ListUI", pascalCasingService.Transform("list UI"));
            Assert.Equal("ListUI", pascalCasingService.Transform("List_UI"));
        }

        [Fact]
        public void Test_TransformToCamelCase()
        {
            var camelCasingService = new TextCasing() { Type = CasingType.Camel };

            Assert.Equal("listDetail", camelCasingService.Transform("ListDetail"));
            Assert.Equal("listDetail", camelCasingService.Transform("List_Detail"));
            Assert.Equal("listDetail", camelCasingService.Transform("List Detail"));
            Assert.Equal("listDetail", camelCasingService.Transform(" List Detail "));
            Assert.Equal("listDetail1", camelCasingService.Transform("list detail 1"));
            Assert.Equal("listUI", camelCasingService.Transform("ListUI"));
            Assert.Equal("listUI", camelCasingService.Transform("list UI"));
            Assert.Equal("listUI", camelCasingService.Transform("List_UI"));
        }

        [Fact]
        public void Test_TransformToLowerCase()
        {
            var lowerCasingService = new TextCasing() { Type = CasingType.Lower };

            Assert.Equal("listdetail", lowerCasingService.Transform("ListDetail"));
            Assert.Equal("listdetail", lowerCasingService.Transform("List_Detail"));
            Assert.Equal("listdetail", lowerCasingService.Transform("List Detail"));
            Assert.Equal("listdetail", lowerCasingService.Transform(" List Detail "));
            Assert.Equal("listdetail1", lowerCasingService.Transform("list detail 1"));
            Assert.Equal("listui", lowerCasingService.Transform("ListUI"));
            Assert.Equal("listui", lowerCasingService.Transform("list UI"));
            Assert.Equal("listui", lowerCasingService.Transform("List_UI"));
        }
    }
}
