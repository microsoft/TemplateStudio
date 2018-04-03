// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Validation;
using Microsoft.Templates.UI.Services;

namespace Microsoft.Templates.UI.Validators
{
    public class HamburgerMenuValidator : IValidator
    {
        // TODO - Change when  xxxxxxxxxxxxx  Set the last hamburger menu version
        private readonly Version lastHamburgerMenuControlVersion = new Version("2.0.18088.1");

        public ValidationResult Validate()
        {
            var result = new ValidationResult();
            var projectMetadata = ProjectMetadataService.GetProjectMetadata();

            var templatesVersion = GetVersion(GenContext.ToolBox.TemplatesVersion);
            var projectVersion = GetVersion(projectMetadata.TemplatesVersion);
            var projectType = projectMetadata.ProjectType;

            if (projectVersion <= lastHamburgerMenuControlVersion
                && templatesVersion > lastHamburgerMenuControlVersion
                && projectType == "SplitView"
                && CheckHamburgerMenuControl())
            {
                var message = string.Format(
                    Resources.StringRes.ValidatorHamburguerMenuErrorMessage,
                    Core.Configuration.Current.GitHubDocsUrl);

                result.IsValid = false;
                result.ErrorMessages.Add(message);
            }

            return result;
        }

        private Version GetVersion(string stringVersion)
        {
            if (string.IsNullOrEmpty(stringVersion))
            {
                return new Version();
            }

            var projectVersion = stringVersion.TrimStart('v');
            return new Version(projectVersion);
        }

        private bool CheckHamburgerMenuControl()
        {
            var file = Path.Combine(GenContext.Current.ProjectPath, "Views", "ShellPage.xaml");
            var fileContent = GetFileContent(file);
            return fileContent.Contains("controls:HamburgerMenu");
        }

        private string GetFileContent(string file)
        {
            if (!File.Exists(file))
            {
                return string.Empty;
            }

            try
            {
                return File.ReadAllText(file);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
