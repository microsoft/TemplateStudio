// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using System.Linq;

using Microsoft.Templates.Core.Gen;

using Xunit;

namespace Microsoft.Templates.Core.Test
{
    [Collection("Unit Test Templates")]
    [Trait("ExecutionSet", "Minimum")]
    public class LocalizationTest
    {
        private TemplatesFixture _fixture;
        private const string SkipMessage = "Disable while wait for the localized files";

        public LocalizationTest(TemplatesFixture fixture)
        {
            _fixture = fixture;
        }

        private void SetUpFixtureForTesting(string language)
        {
            _fixture.InitializeFixture(language);
        }

        public void Dispose()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("en-US");
        }

        [Fact(Skip=SkipMessage)]
        public void Load_ProjectTemplates_en()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("en-US");
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var projectTemplates = GenContext.ToolBox.Repo.GetProjectTypes().ToList();

            MetadataInfo template = projectTemplates != null && projectTemplates.Count > 0 ? projectTemplates[0] : null;

            Assert.NotNull(template);
            Assert.Equal<string>("Test Project Type", template.DisplayName);
            Assert.Equal<string>("Test Project Type Summary", template.Summary);
            Assert.Equal<string>("Test Project Type Description", template.Description);
        }

        [Fact(Skip=SkipMessage)]
        public void Load_ProjectTemplates_es()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("es-ES");
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var projectTemplates = GenContext.ToolBox.Repo.GetProjectTypes().ToList();
            MetadataInfo template = projectTemplates != null && projectTemplates.Count > 0 ? projectTemplates[0] : null;

            Assert.NotNull(template);
            Assert.Equal<string>("Proyecto de prueba", template.DisplayName);
            Assert.Equal<string>("Resumen del proyecto de prueba", template.Summary);
            Assert.Equal<string>("Descripción del proyecto de prueba", template.Description);
        }

        [Fact(Skip=SkipMessage)]
        public void Load_ProjectTemplates_fr()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("fr-FR");
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var projectTemplates = GenContext.ToolBox.Repo.GetProjectTypes().ToList();
            MetadataInfo template = projectTemplates != null && projectTemplates.Count > 0 ? projectTemplates[0] : null;

            Assert.NotNull(template);
            Assert.Equal<string>("Test Project Type Base", template.DisplayName);
            Assert.Equal<string>("Test Project Type Base Summary", template.Summary);
            Assert.Equal<string>("Test Project Type Base Description", template.Description);
        }

        [Fact(Skip=SkipMessage)]
        public void Load_ProjectTemplates_unknown()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("xx-XX");
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var projectTemplates = GenContext.ToolBox.Repo.GetProjectTypes().ToList();
            MetadataInfo template = projectTemplates != null && projectTemplates.Count > 0 ? projectTemplates[0] : null;

            Assert.NotNull(template);
            Assert.Equal<string>("Test Project Type Base", template.DisplayName);
            Assert.Equal<string>("Test Project Type Base Summary", template.Summary);
            Assert.Equal<string>("Test Project Type Base Description", template.Description);
        }

        [Fact(Skip=SkipMessage)]
        public void Load_FrameworkTempletes_en()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("en-US");
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var frameworkTemplates = GenContext.ToolBox.Repo.GetFrameworks().ToList();
            MetadataInfo template = frameworkTemplates != null && frameworkTemplates.Count > 0 ? frameworkTemplates[0] : null;

            Assert.NotNull(template);
            Assert.Equal<string>("Test Framework", template.DisplayName);
            Assert.Equal<string>("Test Framework Summary", template.Summary);
            Assert.Equal<string>("Test Framework Description", template.Description);
        }

        [Fact(Skip=SkipMessage)]
        public void Load_FrameworkTempletes_es()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("es-ES");
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var frameworkTemplates = GenContext.ToolBox.Repo.GetFrameworks().ToList();
            MetadataInfo template = frameworkTemplates != null && frameworkTemplates.Count > 0 ? frameworkTemplates[0] : null;

            Assert.NotNull(template);
            Assert.Equal<string>("Framework de prueba", template.DisplayName);
            Assert.Equal<string>("Resumen de Framework de prueba", template.Summary);
            Assert.Equal<string>("Descripción de Framework de prueba", template.Description);
        }

        [Fact(Skip=SkipMessage)]
        public void Load_FrameworkTempletes_fr()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("fr-FR");
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var frameworkTemplates = GenContext.ToolBox.Repo.GetFrameworks().ToList();
            MetadataInfo template = frameworkTemplates != null && frameworkTemplates.Count > 0 ? frameworkTemplates[0] : null;

            Assert.NotNull(template);
            Assert.Equal<string>("Test Framework Base", template.DisplayName);
            Assert.Equal<string>("Test Framework Base Summary", template.Summary);
            Assert.Equal<string>("Test Framework Base Description", template.Description);
        }

        [Fact(Skip=SkipMessage)]
        public void Load_FrameworkTempletes_unknown()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("xx-XX");
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var frameworkTemplates = GenContext.ToolBox.Repo.GetFrameworks().ToList();
            MetadataInfo template = frameworkTemplates != null && frameworkTemplates.Count > 0 ? frameworkTemplates[0] : null;

            Assert.NotNull(template);
            Assert.Equal<string>("Test Framework Base", template.DisplayName);
            Assert.Equal<string>("Test Framework Base Summary", template.Summary);
            Assert.Equal<string>("Test Framework Base Description", template.Description);
        }

        [Fact(Skip=SkipMessage)]
        public void Load_PageTempletes_es()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("es-ES");
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var template = GenContext.ToolBox.Repo.GetAll().FirstOrDefault(t => t.Identity == "Microsoft.UWPTemplates.Test.PageTemplate.CSharp");

            Assert.NotNull(template);
            Assert.Equal<string>("Microsoft España", template.Author);
            Assert.Equal<string>("Página en Blanco", template.Name);
            Assert.Equal<string>("Está en Español...", template.Description);
            Assert.Equal<string>("Descripción del proyecto de prueba", template.GetRichDescription());
        }

        [Fact(Skip=SkipMessage)]
        public void Load_PageTempletes_en()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("en-US");
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var template = GenContext.ToolBox.Repo.GetAll().FirstOrDefault(t => t.Identity == "Microsoft.UWPTemplates.Test.PageTemplate.CSharp");

            Assert.NotNull(template);
            Assert.Equal<string>("Microsoft USA", template.Author);
            Assert.Equal<string>("Blank US", template.Name);
            Assert.Equal<string>("US English...", template.Description);
            Assert.Equal<string>("US description", template.GetRichDescription());
        }

        [Fact(Skip=SkipMessage)]
        public void Load_PageTempletes_fr()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("fr-FR");
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var template = GenContext.ToolBox.Repo.GetAll().FirstOrDefault(t => t.Identity == "Microsoft.UWPTemplates.Test.PageTemplate.CSharp");

            Assert.NotNull(template);
            Assert.Equal<string>("Microsoft", template.Author);
            Assert.Equal<string>("PageTemplate", template.Name);
            Assert.Equal<string>("Generic description...", template.Description);
            Assert.Equal<string>("Generic description", template.GetRichDescription());
        }

        [Fact(Skip=SkipMessage)]
        public void Load_PageTempletes_unknown()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("xx-XX");
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var template = GenContext.ToolBox.Repo.GetAll().FirstOrDefault(t => t.Identity == "Microsoft.UWPTemplates.Test.PageTemplate.CSharp");

            Assert.NotNull(template);
            Assert.Equal<string>("Microsoft", template.Author);
            Assert.Equal<string>("PageTemplate", template.Name);
            Assert.Equal<string>("Generic description...", template.Description);
            Assert.Equal<string>("Generic description", template.GetRichDescription());
        }

        [Fact(Skip=SkipMessage)]
        public void Load_FeatureTempletes_es()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("es-ES");
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var template = GenContext.ToolBox.Repo.GetAll().FirstOrDefault(t => t.Identity == "Microsoft.UWPTemplates.Test.FeatureTemplate.CSharp");

            Assert.NotNull(template);
            Assert.Equal<string>("Microsoft España", template.Author);
            Assert.Equal<string>("Feature de prueba", template.Name);
            Assert.Equal<string>("Está en Español (Feature)...", template.Description);
            Assert.Equal<string>("Descripción de la Feature de prueba", template.GetRichDescription());
        }

        [Fact(Skip=SkipMessage)]
        public void Load_FeatureTempletes_en()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("en-US");
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var template = GenContext.ToolBox.Repo.GetAll().FirstOrDefault(t => t.Identity == "Microsoft.UWPTemplates.Test.FeatureTemplate.CSharp");

            Assert.NotNull(template);
            Assert.Equal<string>("Microsoft USA", template.Author);
            Assert.Equal<string>("Feature US", template.Name);
            Assert.Equal<string>("Feature US English...", template.Description);
            Assert.Equal<string>("US Feature description", template.GetRichDescription());
        }

        [Fact(Skip=SkipMessage)]
        public void Load_FeatureTempletes_fr()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("fr-FR");
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var template = GenContext.ToolBox.Repo.GetAll().FirstOrDefault(t => t.Identity == "Microsoft.UWPTemplates.Test.FeatureTemplate.CSharp");

            Assert.NotNull(template);
            Assert.Equal<string>("Microsoft", template.Author);
            Assert.Equal<string>("FeatureTemplate", template.Name);
            Assert.Equal<string>("Generic Feature description...", template.Description);
            Assert.Equal<string>("Generic Feature MD description", template.GetRichDescription());
        }

        [Fact(Skip=SkipMessage)]
        public void Load_FeatureTempletes_unknown()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("xx-XX");
            SetUpFixtureForTesting(ProgrammingLanguages.CSharp);

            var template = GenContext.ToolBox.Repo.GetAll().FirstOrDefault(t => t.Identity == "Microsoft.UWPTemplates.Test.FeatureTemplate.CSharp");

            Assert.NotNull(template);
            Assert.Equal<string>("Microsoft", template.Author);
            Assert.Equal<string>("FeatureTemplate", template.Name);
            Assert.Equal<string>("Generic Feature description...", template.Description);
            Assert.Equal<string>("Generic Feature MD description", template.GetRichDescription());
        }
    }
}
