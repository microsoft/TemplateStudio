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

namespace Microsoft.Templates.Core.Test.Composition
{
    public class CompositionQueryTest
    {
        [Fact]
        public void Parse()
        {
            var query = "uct.framework==framework&uct.type!=Page&$name==Map";
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
            var query = "uct.framework==framework&uct.type";
            var result = CompositionQuery.Parse(query);

            Assert.Collection(result.Items,
                r1 =>
                {
                    Assert.Equal(r1.Field, "uct.framework");
                });
        }

        [Fact]
        public void Execute()
        {
            var data = GetFactData(1).FirstOrDefault();

            var target = CompositionQuery.Parse("Identity==item0&item0-tag2==item0-tagVal2");
            var result = target.Execute(data, null);

            Assert.True(result);
        }

        [Fact]
        public void Execute_WithContext()
        {
            var data = GetFactData(4).ToList();

            var target = CompositionQuery.Parse("Identity==item0&item0-tag2==item0-tagVal2&$Name==item2-name");
            var result = target.Execute(data[0], data.Skip(1).ToList());

            Assert.True(result);
        }

        [Fact]
        public void Execute_NotEquals()
        {
            var data = GetFactData(1).FirstOrDefault();

            var target = CompositionQuery.Parse("Identity==item0&item0-tag2!=item0-tagVal1");
            var result = target.Execute(data, null);

            Assert.True(result);
        }

        [Fact]
        public void Execute_MultiValue()
        {
            var data = new FakeTemplateInfo
            {
                Identity = "item",
                Name = "item-name"
            };
            data.AddTag($"item-tag", $"item-tagVal1|item-tagVal2");

            var target = CompositionQuery.Parse("Identity==item&item-tag==item-tagVal2");
            var result = target.Execute(data, null);

            Assert.True(result);
        }

        private IEnumerable<ITemplateInfo> GetFactData(int items)
        {
            for (int i = 0; i < items; i++)
            {
                var templateInfo = new FakeTemplateInfo
                {
                    Identity = $"item{i}",
                    Name = $"item{i}-name"
                };
                templateInfo.AddTag($"item{i}-tag1", $"item{i}-tagVal1");
                templateInfo.AddTag($"item{i}-tag2", $"item{i}-tagVal2");

                yield return templateInfo;
            }
        }
    }

    public class FakeTemplateInfo : ITemplateInfo
    {
        private readonly Dictionary<string, string> _tags = new Dictionary<string, string>();

        public string Identity { get; set; }
        public string Name { get; set; }
        public IReadOnlyDictionary<string, string> Tags => _tags;
        public string GroupIdentity { get; set; }

        public void AddTag(string key, string value)
        {
            _tags.Add(key, value);
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
    }
}
