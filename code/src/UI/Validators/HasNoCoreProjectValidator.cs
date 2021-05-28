// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Helpers;
using Microsoft.Templates.Core.Validation;
using ValidationResult = Microsoft.Templates.Core.Validation.ValidationResult;

namespace Microsoft.Templates.UI.Validators
{
    public class HasNoCoreProjectValidator : IBreakingChangeValidator
    {
        public Version BreakingVersion { get; }

        public HasNoCoreProjectValidator()
        {
            // This is last version with no core project in templates
            var version = Core.Configuration.Current.BreakingChangesVersions.FirstOrDefault(c => c.Name == "HasNoCoreProject")?.BreakingVersion;
            BreakingVersion = version ?? new Version();
        }

        public ValidationResult Validate()
        {
            var result = new ValidationResult();

            if (!HasCoreProject())
            {
                var message = new ValidationMessage
                {
                    Message = Resources.StringRes.ValidatorHasNoCoreProjectMessage,
                    Url = string.Format(Resources.StringRes.ValidatorHasNoCoreProjectLink, Core.Configuration.Current.GitHubDocsUrl),
                    HyperLinkMessage = Resources.StringRes.ValidatorHasNoCoreProjectLinkMessage,
                };

                result.IsValid = false;
                result.ErrorMessages.Add(message);
            }

            return result;
        }

        private bool HasCoreProject()
        {
            var extension = (GenContext.ToolBox.Shell.Project.GetActiveProjectLanguage() == ProgrammingLanguages.CSharp) ? "csproj" : "vbproj";

            var uwpProjectName = GenContext.ToolBox.Shell.Project.GetActiveProjectName();
            var uwpProjectFolder = GenContext.ToolBox.Shell.Project.GetActiveProjectPath();
            var uwpProjectPath = Path.Combine(uwpProjectFolder, $"{uwpProjectName}.{extension}");

            var coreProjectName = $"{uwpProjectName}.Core";
            var coreProjectPath = Path.Combine(Directory.GetParent(uwpProjectFolder).FullName, coreProjectName, $"{coreProjectName}.{extension}");
            var uwpFileContent = FileHelper.GetFileContent(uwpProjectPath);

            if (File.Exists(coreProjectPath) && uwpFileContent.Contains($"<ProjectReference Include=\"..\\{coreProjectName}\\{coreProjectName}.{extension}\">"))
            {
                var fileContent = FileHelper.GetFileContent(coreProjectPath);
                if (fileContent.Contains("netstandard2.0"))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
