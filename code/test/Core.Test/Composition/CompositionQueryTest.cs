// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core.Composition;

using Xunit;

namespace Microsoft.Templates.Core.Test.Composition
{
    [Trait("ExecutionSet", "Minimum")]
    public class CompositionQueryTest
    {
        [Fact]
        public void ParseInvalidQueries()
        {
            var query1 = "wts.frontendframework = framework & wts.type != Page&$name == Map";

            Assert.Throws<InvalidCompositionQueryException>(() =>
            {
               var result = CompositionQuery.Parse(query1);
            });

            var query2 = "wts.frontendframework= framework &wts.type ! Page&$name != Map";

            Assert.Throws<InvalidCompositionQueryException>(() =>
            {
                var result = CompositionQuery.Parse(query2);
            });

            var query3 = "wts.frontendframework==framework&wts.type!Page&$name == Map";

            Assert.Throws<InvalidCompositionQueryException>(() =>
            {
                var result = CompositionQuery.Parse(query3);
            });

            var query4 = "wts.frontendframework   == framework & wts.type=Page& $name == Map";

            Assert.Throws<InvalidCompositionQueryException>(() =>
            {
                var result = CompositionQuery.Parse(query4);
            });
        }

        [Fact]
        public void Parse()
        {
            var query = "wts.frontendframework == framework & wts.type != Page&$name == Map";
            var result = CompositionQuery.Parse(query);

            Assert.Collection(
                result.Items,
                r1 =>
                {
                    Assert.Equal("wts.frontendframework", r1.Field);
                    Assert.Equal(QueryOperator.Equals, r1.Operator);
                    Assert.Equal("framework", r1.Value);
                    Assert.False(r1.IsContext);
                },
                r2 =>
                {
                    Assert.Equal("wts.type", r2.Field);
                    Assert.Equal(QueryOperator.NotEquals, r2.Operator);
                    Assert.Equal("Page", r2.Value);
                    Assert.False(r2.IsContext);
                },
                r3 =>
                {
                    Assert.Equal("name", r3.Field);
                    Assert.Equal(QueryOperator.Equals, r3.Operator);
                    Assert.Equal("Map", r3.Value);
                    Assert.True(r3.IsContext);
                });
        }

        [Fact]
        public void Parse_MultipleItems()
        {
            var query = new string[]
            {
                "wts.frontendframework == framework",
                "wts.type != Page",
                "$name == Map",
            };

            var result = CompositionQuery.Parse(query);

            Assert.Collection(
                result.Items,
                r1 =>
                {
                    Assert.Equal("wts.frontendframework", r1.Field);
                    Assert.Equal(QueryOperator.Equals, r1.Operator);
                    Assert.Equal("framework", r1.Value);
                    Assert.False(r1.IsContext);
                },
                r2 =>
                {
                    Assert.Equal("wts.type", r2.Field);
                    Assert.Equal(QueryOperator.NotEquals, r2.Operator);
                    Assert.Equal("Page", r2.Value);
                    Assert.False(r2.IsContext);
                },
                r3 =>
                {
                    Assert.Equal("name", r3.Field);
                    Assert.Equal(QueryOperator.Equals, r3.Operator);
                    Assert.Equal("Map", r3.Value);
                    Assert.True(r3.IsContext);
                });
        }

        [Fact]
        public void Parse_NoValueInParam()
        {
            var query = "wts.frontendframework == framework & wts.type";

            Assert.Throws<InvalidCompositionQueryException>(() =>
            {
                var result = CompositionQuery.Parse(query);
            });
        }

        [Fact]
        public void Match()
        {
            var data = GetFactData();

            var target = CompositionQuery.Parse("identity==item-identity&tag2==tagVal2");
            var result = target.Match(data, null);

            Assert.True(result);
        }

        [Fact]
        public void Match_WithContext()
        {
            var data = GetFactData();
            var target = CompositionQuery.Parse("identity==item-identity&tag2==tagVal2&$name==context-name");
            var context = new QueryablePropertyDictionary { new QueryableProperty("name", "context-name") };

            var result = target.Match(data, context);

            Assert.True(result);
        }

        [Fact]
        public void NoMatch_WithContext()
        {
            var data = GetFactData();
            var target = CompositionQuery.Parse("identity!=item-identity&tag2==tagVal2&$name==context-name");
            var context = new QueryablePropertyDictionary { new QueryableProperty("name", "context-name") };

            var result = target.Match(data, context);

            Assert.False(result);
        }

        [Fact]
        public void Match_WithContext_NotEquals()
        {
            var data = GetFactData();
            var target = CompositionQuery.Parse("identity==item-identity&tag2==tagVal2&$name!=context-name");
            var context = new QueryablePropertyDictionary { new QueryableProperty("name", "otherName") };

            var result = target.Match(data, context);

            Assert.True(result);
        }


        [Fact]
        public void Match_WithContext_NotEquals_Empty()
        {
            var data = GetFactData();
            var target = CompositionQuery.Parse("identity==item-identity&tag2==tagVal2&$name!=context-name");
            var context = new QueryablePropertyDictionary { new QueryableProperty("name", "") };

            var result = target.Match(data, context);

            Assert.True(result);
        }

        [Fact]
        public void Match_NotEquals()
        {
            var data = GetFactData();
            var target = CompositionQuery.Parse("identity==item-identity&tag2!=tagVal1");
            var result = target.Match(data, null);

            Assert.True(result);
        }

        [Fact]
        public void Match_MultiValue()
        {
            var data = GetFactData();
            var target = CompositionQuery.Parse("identity==item-identity&tag3==tag3Val1");
            var result = target.Match(data, null);

            Assert.True(result);
        }

        [Fact]
        public void Match_MultiValueBoth()
        {
            var data = GetFactData();
            var target = CompositionQuery.Parse("identity==item-identity&tag3==tag3Val1|tag3Val3");
            var result = target.Match(data, null);

            Assert.True(result);
        }

        private ITemplateInfo GetFactData()
        {
            var templateInfo = new FakeTemplateInfo
            {
                Identity = $"item-identity",
                Name = $"item-name",
            };

            templateInfo.AddTag($"tag1", $"tagVal1");
            templateInfo.AddTag($"tag2", $"tagVal2");
            templateInfo.AddTag($"tag3", $"tag3Val1|tag3Val2");

            return templateInfo;
        }
    }
}
