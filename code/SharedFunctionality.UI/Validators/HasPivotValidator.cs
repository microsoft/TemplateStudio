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
using Microsoft.Templates.SharedResources;

namespace Microsoft.Templates.UI.Validators
{
    public class HasPivotValidator : IBreakingChangeValidator
    {
        public Version BreakingVersion { get; }

        public HasPivotValidator()
        {
            // This is last version with Hamburguer menu control in templates
            var version = Core.Configuration.Current.BreakingChangesVersions.FirstOrDefault(c => c.Name == "HasPivot")?.BreakingVersion;
            BreakingVersion = version ?? new Version();
        }

        public ValidationResult Validate()
        {
            var result = new ValidationResult();
            var projectType = ProjectMetadataService.GetProjectMetadata(GenContext.ToolBox.Shell.Project.GetActiveProjectPath()).ProjectType;

            if (projectType == "TabbedPivot" && HasPivot())
            {
                var message = new ValidationMessage
                {
                    Message = Resources.ValidatorHasPivotMessage,
                    Url = string.Format(Resources.ValidatorHasPivotLink, Core.Configuration.Current.GitHubDocsUrl),
                    HyperLinkMessage = Resources.ValidatorHasPivotLinkMessage,
                };

                result.IsValid = false;
                result.ErrorMessages.Add(message);
            }

            return result;
        }

        private bool HasPivot()
        {
            var filePath = Path.Combine(GenContext.ToolBox.Shell.Project.GetActiveProjectPath(), "Views", "PivotPage.xaml");
            var fileContent = FileHelper.GetFileContent(filePath);
            return fileContent.Contains("<Pivot");
        }
    }
}
