// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Microsoft.Templates.Core;
using Microsoft.Templates.UI.Services;
using Xunit;

namespace Microsoft.UI.Test
{
    [Collection("UI")]
    [Trait("ExecutionSet", "Minimum")]
    public class ValidationServiceTest : IClassFixture<TemplatesFixture>
    {
        private TemplatesFixture _fixture;

        public ValidationServiceTest(TemplatesFixture fixture)
        {
            _fixture = fixture;
            _fixture.InitializeFixture(Platforms.Uwp, ProgrammingLanguages.CSharp);
        }

        [Fact]
        public void ValidateTemplateName()
        {
            ValidationService.Initialize(() => new List<string>()
            {
                "Main",
                "Settings",
                "SettingsStorage"
            });

            // Add a template that can choose the name
            Assert.True(ValidationService.ValidateTemplateName("Map", true, true).IsValid);

            // Add a template that can choose the name and this name is already used
            Assert.False(ValidationService.ValidateTemplateName("Main", true, true).IsValid);

            // Add a template that can not choose the name
            Assert.True(ValidationService.ValidateTemplateName("UriScheme", false, true).IsValid);

            // Add a template that can not choose the name and this name is already used
            Assert.False(ValidationService.ValidateTemplateName("SettingsStorage", false, true).IsValid);

            // Add a template that can choose the name but we don't know if it is already used (Right click)
            Assert.True(ValidationService.ValidateTemplateName("Main", true, false).IsValid);
        }

        [Fact]
        public void InferTemplateName()
        {
            ValidationService.Initialize(() => new List<string>()
            {
                "Main",
                "Map",
                "Settings",
                "SettingsStorage"
            });

            // Infer name for a template that you can choose the name
            Assert.True(ValidationService.InferTemplateName("Blank", true, true) == "Blank");

            // Infer name for a template that you can choose the name and this name is already used
            Assert.True(ValidationService.InferTemplateName("Map", true, true) == "Map1");

            // Infer name for a template that you can not choose the name. This template can not be added for twice.
            Assert.True(ValidationService.InferTemplateName("UriScheme", false, false) == "UriScheme");

            // Infer name for a template that you can chose the name but you can not known if this name is already used (right click).
            Assert.True(ValidationService.InferTemplateName("Main", false, true) == "Main");

            // Infer name for a template that you can not chose the name but you can not known if this name is already used (right click).
            Assert.True(ValidationService.InferTemplateName("SettingsStorage", false, false) == "SettingsStorage");
        }
    }
}
