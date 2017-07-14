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

using Microsoft.Templates.Core.Gen;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Microsoft.Templates.Core.Test
{
    [Collection("Unit Test Templates")]
    public class LocalizationTest
    {

        private TemplatesFixture _fixture;

        public LocalizationTest(TemplatesFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Load_ProjectTemplates_en()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("en-US");
            var projectTemplates = GenContext.ToolBox.Repo.GetProjectTypes().ToList();
            MetadataInfo template = projectTemplates != null && projectTemplates.Count > 0 ? projectTemplates[0] : null;
            Assert.NotNull(template);
            Assert.Equal<string>("Test Project Type", template.DisplayName);
            Assert.Equal<string>("Test Project Type Summary", template.Summary);
            Assert.Equal<string>("Test Project Type Description", template.Description);
        }

        [Fact]
        public void Load_ProjectTemplates_es()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("es-ES");
            var projectTemplates = GenContext.ToolBox.Repo.GetProjectTypes().ToList();
            MetadataInfo template = projectTemplates != null && projectTemplates.Count > 0 ? projectTemplates[0] : null;
            Assert.NotNull(template);
            Assert.Equal<string>("Proyecto de prueba", template.DisplayName);
            Assert.Equal<string>("Resumen del proyecto de prueba", template.Summary);
            Assert.Equal<string>("Descripción del proyecto de prueba", template.Description);
        }

        [Fact]
        public void Load_ProjectTemplates_fr()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("fr-FR");
            var projectTemplates = GenContext.ToolBox.Repo.GetProjectTypes().ToList();
            MetadataInfo template = projectTemplates != null && projectTemplates.Count > 0 ? projectTemplates[0] : null;
            Assert.NotNull(template);
            Assert.Equal<string>("Test Project Type Base", template.DisplayName);
            Assert.Equal<string>("Test Project Type Base Summary", template.Summary);
            Assert.Equal<string>("Test Project Type Base Description", template.Description);
        }

        [Fact]
        public void Load_ProjectTemplates_unknown()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("xx-XX");
            var projectTemplates = GenContext.ToolBox.Repo.GetProjectTypes().ToList();
            MetadataInfo template = projectTemplates != null && projectTemplates.Count > 0 ? projectTemplates[0] : null;
            Assert.NotNull(template);
            Assert.Equal<string>("Test Project Type Base", template.DisplayName);
            Assert.Equal<string>("Test Project Type Base Summary", template.Summary);
            Assert.Equal<string>("Test Project Type Base Description", template.Description);
        }

        [Fact]
        public void Load_FrameworkTempletes_en()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("en-US");
            var frameworkTemplates = GenContext.ToolBox.Repo.GetFrameworks().ToList();
            MetadataInfo template = frameworkTemplates != null && frameworkTemplates.Count > 0 ? frameworkTemplates[0] : null;
            Assert.NotNull(template);
            Assert.Equal<string>("Test Framework", template.DisplayName);
            Assert.Equal<string>("Test Framework Summary", template.Summary);
            Assert.Equal<string>("Test Framework Description", template.Description);
        }

        [Fact]
        public void Load_FrameworkTempletes_es()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("es-ES");
            var frameworkTemplates = GenContext.ToolBox.Repo.GetFrameworks().ToList();
            MetadataInfo template = frameworkTemplates != null && frameworkTemplates.Count > 0 ? frameworkTemplates[0] : null;
            Assert.NotNull(template);
            Assert.Equal<string>("Framework de prueba", template.DisplayName);
            Assert.Equal<string>("Resumen de Framework de prueba", template.Summary);
            Assert.Equal<string>("Descripción de Framework de prueba", template.Description);
        }

        [Fact]
        public void Load_FrameworkTempletes_fr()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("fr-FR");
            var frameworkTemplates = GenContext.ToolBox.Repo.GetFrameworks().ToList();
            MetadataInfo template = frameworkTemplates != null && frameworkTemplates.Count > 0 ? frameworkTemplates[0] : null;
            Assert.NotNull(template);
            Assert.Equal<string>("Test Framework Base", template.DisplayName);
            Assert.Equal<string>("Test Framework Base Summary", template.Summary);
            Assert.Equal<string>("Test Framework Base Description", template.Description);
        }

        [Fact]
        public void Load_FrameworkTempletes_unknown()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("xx-XX");
            var frameworkTemplates = GenContext.ToolBox.Repo.GetFrameworks().ToList();
            MetadataInfo template = frameworkTemplates != null && frameworkTemplates.Count > 0 ? frameworkTemplates[0] : null;
            Assert.NotNull(template);
            Assert.Equal<string>("Test Framework Base", template.DisplayName);
            Assert.Equal<string>("Test Framework Base Summary", template.Summary);
            Assert.Equal<string>("Test Framework Base Description", template.Description);
        }

        [Fact]
        public void Load_PageTempletes_es()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("es-ES");
            this._fixture = new TemplatesFixture();
            var template = GenContext.ToolBox.Repo.GetAll().Where(t => t.Identity == "Microsoft.UWPTemplates.Test.PageTemplate.CSharp").FirstOrDefault();
            Assert.NotNull(template);
            Assert.Equal<string>("Microsoft España", template.Author);
            Assert.Equal<string>("Página en Blanco", template.Name);
            Assert.Equal<string>("Está en Español...", template.Description);
            Assert.Equal<string>("Descripción del proyecto de prueba", template.GetRichDescription());
        }

        [Fact]
        public void Load_PageTempletes_en()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("en-US");
            this._fixture = new TemplatesFixture();
            var template = GenContext.ToolBox.Repo.GetAll().Where(t => t.Identity == "Microsoft.UWPTemplates.Test.PageTemplate.CSharp").FirstOrDefault();
            Assert.NotNull(template);
            Assert.Equal<string>("Microsoft USA", template.Author);
            Assert.Equal<string>("Blank US", template.Name);
            Assert.Equal<string>("US English...", template.Description);
            Assert.Equal<string>("US description", template.GetRichDescription());
        }

        [Fact]
        public void Load_PageTempletes_fr()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("fr-FR");
            this._fixture = new TemplatesFixture();
            var template = GenContext.ToolBox.Repo.GetAll().Where(t => t.Identity == "Microsoft.UWPTemplates.Test.PageTemplate.CSharp").FirstOrDefault();
            Assert.NotNull(template);
            Assert.Equal<string>("Microsoft", template.Author);
            Assert.Equal<string>("PageTemplate", template.Name);
            Assert.Equal<string>("Generic description...", template.Description);
            Assert.Equal<string>("Generic description", template.GetRichDescription());
        }

        [Fact]
        public void Load_PageTempletes_unknown()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("xx-XX");
            this._fixture = new TemplatesFixture();
            var template = GenContext.ToolBox.Repo.GetAll().Where(t => t.Identity == "Microsoft.UWPTemplates.Test.PageTemplate.CSharp").FirstOrDefault();
            Assert.NotNull(template);
            Assert.Equal<string>("Microsoft", template.Author);
            Assert.Equal<string>("PageTemplate", template.Name);
            Assert.Equal<string>("Generic description...", template.Description);
            Assert.Equal<string>("Generic description", template.GetRichDescription());
        }

        [Fact]
        public void Load_FeatureTempletes_es()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("es-ES");
            this._fixture = new TemplatesFixture();
            var template = GenContext.ToolBox.Repo.GetAll().Where(t => t.Identity == "Microsoft.UWPTemplates.Test.FeatureTemplate.CSharp").FirstOrDefault();
            Assert.NotNull(template);
            Assert.Equal<string>("Microsoft España", template.Author);
            Assert.Equal<string>("Feature de prueba", template.Name);
            Assert.Equal<string>("Está en Español (Feature)...", template.Description);
            Assert.Equal<string>("Descripción de la Feature de prueba", template.GetRichDescription());
        }

        [Fact]
        public void Load_FeatureTempletes_en()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("en-US");
            this._fixture = new TemplatesFixture();
            var template = GenContext.ToolBox.Repo.GetAll().Where(t => t.Identity == "Microsoft.UWPTemplates.Test.FeatureTemplate.CSharp").FirstOrDefault();
            Assert.NotNull(template);
            Assert.Equal<string>("Microsoft USA", template.Author);
            Assert.Equal<string>("Feature US", template.Name);
            Assert.Equal<string>("Feature US English...", template.Description);
            Assert.Equal<string>("US Feature description", template.GetRichDescription());
        }

        [Fact]
        public void Load_FeatureTempletes_fr()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("fr-FR");
            this._fixture = new TemplatesFixture();
            var template = GenContext.ToolBox.Repo.GetAll().Where(t => t.Identity == "Microsoft.UWPTemplates.Test.FeatureTemplate.CSharp").FirstOrDefault();
            Assert.NotNull(template);
            Assert.Equal<string>("Microsoft", template.Author);
            Assert.Equal<string>("FeatureTemplate", template.Name);
            Assert.Equal<string>("Generic Feature description...", template.Description);
            Assert.Equal<string>("Generic Feature MD description", template.GetRichDescription());
        }

        [Fact]
        public void Load_FeatureTempletes_unknown()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("xx-XX");
            this._fixture = new TemplatesFixture();
            var template = GenContext.ToolBox.Repo.GetAll().Where(t => t.Identity == "Microsoft.UWPTemplates.Test.FeatureTemplate.CSharp").FirstOrDefault();
            Assert.NotNull(template);
            Assert.Equal<string>("Microsoft", template.Author);
            Assert.Equal<string>("FeatureTemplate", template.Name);
            Assert.Equal<string>("Generic Feature description...", template.Description);
            Assert.Equal<string>("Generic Feature MD description", template.GetRichDescription());
        }

    }

}
