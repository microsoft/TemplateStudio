﻿// Licensed to the .NET Foundation under one or more agreements.
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
    public class HasOldNavigationViewValidator : IBreakingChangeValidator
    {
        public Version BreakingVersion { get; }

        public HasOldNavigationViewValidator()
        {
            // This is last version with old NavigationView control in templates
            var version = Core.Configuration.Current.BreakingChangesVersions.FirstOrDefault(c => c.Name == "HasOldNavigationView")?.BreakingVersion;
            BreakingVersion = version ?? new Version();
        }

        public ValidationResult Validate()
        {
            var result = new ValidationResult();
            var projectType = ProjectMetadataService.GetProjectMetadata(GenContext.ToolBox.Shell.Project.GetActiveProjectPath()).ProjectType;

            if (projectType == "SplitView" && HasOldNavigationView())
            {
                var message = new ValidationMessage
                {
                    Message = Resources.ValidatorHasOldNavigationViewMessage,
                    Url = string.Format(Resources.ValidatorHasOldNavigationViewLink, Core.Configuration.Current.GitHubDocsUrl),
                    HyperLinkMessage = Resources.ValidatorHasOldNavigationViewLinkMessage,
                };

                result.IsValid = false;
                result.ErrorMessages.Add(message);
            }

            return result;
        }

        private bool HasOldNavigationView()
        {
            var filePath = Path.Combine(GenContext.ToolBox.Shell.Project.GetActiveProjectPath(), "Views", "ShellPage.xaml");
            var fileContent = FileHelper.GetFileContent(filePath);
            return fileContent.Contains("<NavigationView");
        }
    }
}
