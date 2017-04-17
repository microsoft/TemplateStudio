// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core.Composition;

using Xunit;
using Microsoft.TemplateEngine.Utils;

namespace Microsoft.Templates.Core.Test.Composition
{
    public class CompositionQueryTest
    {
        [Fact]
        public void Parse()
        {
            var query = "uct.framework == framework & uct.type != Page&$name == Map";
            var result = CompositionQuery.Parse(query);

            Assert.Collection(result.Items,
                r1 =>
                {
                    Assert.Equal(r1.Field, "uct.framework");
                    Assert.Equal(r1.Operator, QueryOperator.Equals);
                    Assert.Equal(r1.Value, "framework");
                    Assert.False(r1.IsContext);
                },
                r2 =>
                {
                    Assert.Equal(r2.Field, "uct.type");
                    Assert.Equal(r2.Operator, QueryOperator.NotEquals);
                    Assert.Equal(r2.Value, "Page");
                    Assert.False(r2.IsContext);
                },
                r3 =>
                {
                    Assert.Equal(r3.Field, "name");
                    Assert.Equal(r3.Operator, QueryOperator.Equals);
                    Assert.Equal(r3.Value, "Map");
                    Assert.True(r3.IsContext);
                });
        }

        [Fact]
        public void Parse_MultipleItems()
        {
            var query = new string[] 
            {
                "uct.framework == framework",
                "uct.type != Page",
                "$name == Map"
            };
            var result = CompositionQuery.Parse(query);

            Assert.Collection(result.Items,
                r1 =>
                {
                    Assert.Equal(r1.Field, "uct.framework");
                    Assert.Equal(r1.Operator, QueryOperator.Equals);
                    Assert.Equal(r1.Value, "framework");
                    Assert.False(r1.IsContext);
                },
                r2 =>
                {
                    Assert.Equal(r2.Field, "uct.type");
                    Assert.Equal(r2.Operator, QueryOperator.NotEquals);
                    Assert.Equal(r2.Value, "Page");
                    Assert.False(r2.IsContext);
                },
                r3 =>
                {
                    Assert.Equal(r3.Field, "name");
                    Assert.Equal(r3.Operator, QueryOperator.Equals);
                    Assert.Equal(r3.Value, "Map");
                    Assert.True(r3.IsContext);
                });
        }

        [Fact]
        public void Parse_NoValueInParam()
        {
            var query = "uct.framework == framework & uct.type";
            var result = CompositionQuery.Parse(query);

            Assert.Collection(result.Items,
                r1 =>
                {
                    Assert.Equal(r1.Field, "uct.framework");
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
            var context = new QueryablePropertyDictionary
            {
                new QueryableProperty("name", "context-name")
            };

            var target = CompositionQuery.Parse("identity==item-identity&tag2==tagVal2&$name==context-name");
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
                Name = $"item-name"
            };
            templateInfo.AddTag($"tag1", $"tagVal1");
            templateInfo.AddTag($"tag2", $"tagVal2");
            templateInfo.AddTag($"tag3", $"tag3Val1|tag3Val2");

            return templateInfo;
        }
    }

    public class FakeTemplateInfo : ITemplateInfo
    {
        private readonly Dictionary<string, ICacheTag> _tags = new Dictionary<string, ICacheTag>();

        public string Identity { get; set; }
        public string Name { get; set; }
        //public IReadOnlyDictionary<string, string> Tags => _tags;
        public IReadOnlyDictionary<string, ICacheTag> Tags => _tags;
        public string GroupIdentity { get; set; }

        public void AddTag(string key, string value)
        {
            var cacheTag = new CacheTag("", new Dictionary<string, string>(), value);
            _tags.Add(key, cacheTag);
        }

        public string Author => throw new NotImplementedException();

        public string Description => throw new NotImplementedException();

        public IReadOnlyList<string> Classifications => throw new NotImplementedException();

        public string DefaultName => throw new NotImplementedException();

        public Guid GeneratorId => throw new NotImplementedException();

        public string ShortName => throw new NotImplementedException();

        public Guid ConfigMountPointId => throw new NotImplementedException();

        public string ConfigPlace => throw new NotImplementedException();

        public Guid LocaleConfigMountPointId => throw new NotImplementedException();

        public string LocaleConfigPlace => throw new NotImplementedException();

        public int Precedence => throw new NotImplementedException();

        public IReadOnlyDictionary<string, ICacheParameter> CacheParameters => throw new NotImplementedException();

        public IReadOnlyList<ITemplateParameter> Parameters => throw new NotImplementedException();

        public Guid HostConfigMountPointId => throw new NotImplementedException();

        public string HostConfigPlace => throw new NotImplementedException();

        public string ThirdPartyNotices => throw new NotImplementedException();
    }
}
