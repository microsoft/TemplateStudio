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
    public class HasOldMvvmLightLocatorValidator : IBreakingChangeValidator
    {
        public Version BreakingVersion { get; }

        public HasOldMvvmLightLocatorValidator()
        {
            var version = Core.Configuration.Current.BreakingChangesVersions.FirstOrDefault(c => c.Name == "OldMvvmLightLocator")?.BreakingVersion;
            BreakingVersion = version ?? new Version();
        }

        public ValidationResult Validate()
        {
            var result = new ValidationResult();
            var framework = ProjectMetadataService.GetProjectMetadata(GenContext.ToolBox.Shell.Project.GetActiveProjectPath()).Framework;
            if (framework == "MVVMLight" && HasLocatorAsApplicationResource())
            {
                var message = new ValidationMessage
                {
                    Message = Resources.StringRes.ValidatorHasOldMvvmLightLocatorMessage,
                    Url = string.Format(Resources.StringRes.ValidatorHasOldMvvmLightLocatorLink, Core.Configuration.Current.GitHubDocsUrl),
                    HyperLinkMessage = Resources.StringRes.ValidatorHasOldMvvmLightLocatorLinkMessage,
                };

                result.IsValid = false;
                result.ErrorMessages.Add(message);
            }

            return result;
        }

        private bool HasLocatorAsApplicationResource()
        {
            var filePath = Path.Combine(GenContext.ToolBox.Shell.Project.GetActiveProjectPath(), "App.xaml");
            var fileContent = FileHelper.GetFileContent(filePath);
            return fileContent.Contains("<vms:ViewModelLocator x:Key=\"Locator\" />");
        }
    }
}
