using Microsoft.TemplateEngine.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Microsoft.Templates.Core.Test
{
    public class CompositionQueryTest
    {
        [Fact]
        public void Parse()
        {
            var query = "uct.framework==framework&uct.type==Page&$name==Map";
            var result = CompositionQuery.Parse(query);

            Assert.Collection(result.Items,
                r1 =>
                {
                    Assert.Equal(r1.Key, "uct.framework");
                    Assert.Equal(r1.Value, "framework");
                },
                r2 =>
                {
                    Assert.Equal(r2.Key, "uct.type");
                    Assert.Equal(r2.Value, "Page");
                }, 
                r3 =>
                {
                    Assert.Equal(r3.Key, "$name");
                    Assert.Equal(r3.Value, "Map");
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
                    Assert.Equal(r1.Key, "uct.framework");
                    Assert.Equal(r1.Value, "framework");
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
