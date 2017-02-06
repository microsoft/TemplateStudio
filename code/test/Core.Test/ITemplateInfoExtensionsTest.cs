using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.TemplateEngine.Abstractions;
using Xunit;
using Xunit.Sdk;

namespace Microsoft.Templates.Core.Test
{
    public class ITemplateInfoExtensionsTest : IClassFixture<TemplatesFixture>
    {
        private readonly TemplatesFixture _fixture;

        public ITemplateInfoExtensionsTest(TemplatesFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void GetTemplateType_project()
        {
            var target = GetTarget("ProjectTemplate");

            var result = target.GetTemplateType();
            Assert.Equal(TemplateType.Project, result);
        }

        [Fact]
        public void GetTemplateType_page()
        {
            var target = GetTarget("PageTemplate");

            var result = target.GetTemplateType();
            Assert.Equal(TemplateType.Page, result);
        }

        [Fact]
        public void GetTemplateType_feature()
        {
            var target = GetTarget("FeatureTemplate");

            var result = target.GetTemplateType();
            Assert.Equal(TemplateType.Feature, result);
        }

        [Fact]
        public void GetTemplateType_unspecified()
        {
            var target = GetTarget("UnspecifiedTemplate");

            var result = target.GetTemplateType();
            Assert.Equal(TemplateType.Unspecified, result);
        }

        [Fact]
        public void GetIcon()
        {
            var target = GetTarget("ProjectTemplate");

            var result = target.GetIcon();
            var expected = Path.Combine(_fixture.Repository.WorkingFolder, @"Templates\ProjectTemplate", ".template.config",
                "icon.png");
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetIcon_unspecified()
        {
            var target = GetTarget("UnspecifiedTemplate");

            var result = target.GetIcon();
            Assert.Null(result);
        }

        [Fact]
        public void GetFramework()
        {
            var target = GetTarget("ProjectTemplate");

            var result = target.GetFramework();
            Assert.Equal("fx1", result);
        }

        [Fact]
        public void GetFramework_unspecified()
        {
            var target = GetTarget("UnspecifiedTemplate");

            var result = target.GetFramework();
            Assert.Null(result);
        }

        [Fact]
        public void GetVersion()
        {
            var target = GetTarget("ProjectTemplate");

            var result = target.GetVersion();
            Assert.Equal("1.0.0", result);
        }

        [Fact]
        public void GetVersion_unspecified()
        {
            var target = GetTarget("UnspecifiedTemplate");

            var result = target.GetVersion();
            Assert.Null(result);
        }

        [Fact]
        public void GetOrder()
        {
            var target = GetTarget("ProjectTemplate");

            var result = target.GetOrder();
            Assert.Equal(1, result);
        }

        [Fact]
        public void GetOrder_unspecified()
        {
            var target = GetTarget("UnspecifiedTemplate");

            var result = target.GetOrder();
            Assert.Equal(int.MaxValue, result);
        }

        [Fact]
        public void GetLicences()
        {
            var target = GetTarget("ProjectTemplate");

            var result = target.GetLicences().ToList();
            Assert.NotNull(result);

            Assert.Collection(result, 
                e1 => {
                    Assert.Equal("text1", e1.text);
                    Assert.Equal("url1", e1.url);
                    },
                e2 => {
                    Assert.Equal("text2", e2.text);
                    Assert.Equal("url2", e2.url);
                    }
                );
        }

        [Fact]
        public void GetLicences_unspecified()
        {
            var target = GetTarget("UnspecifiedTemplate");

            var result = target.GetLicences().ToList();
            Assert.Equal(0, result.Count);
        }

        private ITemplateInfo GetTarget(string templateName)
        {
            var allTemplates = _fixture.Repository.GetAll();
            var target = allTemplates.FirstOrDefault(t => t.Name == templateName);
            return target;
        }
    }
}