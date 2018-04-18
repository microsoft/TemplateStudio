// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Linq;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Validation;
using Microsoft.Templates.UI.Services;

namespace Microsoft.Templates.UI.Validators
{
    public class HasNavigationViewValidator : IBreakingChangeValidator
    {
        public Version BreakingVersion { get; }

        public HasNavigationViewValidator()
        {
            // This is last version with Hamburguer menu control in templates
            var version = Core.Configuration.Current.BreakingChangesVersions.FirstOrDefault(c => c.Name == "AddNavigationView")?.BreakingVersion;
            BreakingVersion = version ?? new Version();
        }

        public ValidationResult Validate()
        {
            var result = new ValidationResult();
            var projectType = ProjectMetadataService.GetProjectMetadata().ProjectType;

            if (projectType == "SplitView" && !HasNavigationView())
            {
                var message = new ValidationMessage
                {
                    Message = Resources.StringRes.ValidatorHasNavigationViewMessage,
                    Url = string.Format(Resources.StringRes.ValidatorHasNavigationViewLink, Core.Configuration.Current.GitHubDocsUrl),
                    HyperLinkMessage = Resources.StringRes.ValidatorHasNavigationViewLinkMessage,
                };

                result.IsValid = false;
                result.ErrorMessages.Add(message);
            }

            return result;
        }

        private bool HasNavigationView()
        {
            var file = Path.Combine(GenContext.ToolBox.Shell.GetActiveProjectPath(), "Views", "ShellPage.xaml");
            var fileContent = GetFileContent(file);
            return fileContent.Contains("<NavigationView");
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
