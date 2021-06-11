// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Linq;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Helpers;
using Microsoft.Templates.Core.Services;
using Microsoft.Templates.Core.Validation;

namespace Microsoft.Templates.UI.Validators
{
    public class HasHamburgerMenuValidator : IBreakingChangeValidator
    {
        public Version BreakingVersion { get; }

        public HasHamburgerMenuValidator()
        {
            // This is last version with Hamburguer menu control in templates
            var version = Core.Configuration.Current.BreakingChangesVersions.FirstOrDefault(c => c.Name == "HasHamburgerMenu")?.BreakingVersion;
            BreakingVersion = version ?? new Version();
        }

        public ValidationResult Validate()
        {
            var result = new ValidationResult();
            var projectType = ProjectMetadataService.GetProjectMetadata(GenContext.ToolBox.Shell.Project.GetActiveProjectPath()).ProjectType;

            if (projectType == "SplitView" && HasHamburgerMenu())
            {
                var message = new ValidationMessage
                {
                    Message = Resources.StringRes.ValidatorHasHamburgerMenuMessage,
                    Url = string.Format(Resources.StringRes.ValidatorHasHamburgerMenuLink, Core.Configuration.Current.GitHubDocsUrl),
                    HyperLinkMessage = Resources.StringRes.ValidatorHasHamburgerMenuLinkMessage,
                };

                result.IsValid = false;
                result.ErrorMessages.Add(message);
            }

            return result;
        }

        private bool HasHamburgerMenu()
        {
            var filePath = Path.Combine(GenContext.ToolBox.Shell.Project.GetActiveProjectPath(), "Views", "ShellPage.xaml");
            var fileContent = FileHelper.GetFileContent(filePath);
            return fileContent.Contains("<controls:HamburgerMenu");
        }
    }
}
