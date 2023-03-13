// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Services;
using Microsoft.Templates.Core.Validation;
using Microsoft.Templates.UI.Validators;

namespace Microsoft.Templates.UI.Services
{
    public static class BreakingChangesValidatorService
    {
        // add breaking changes validators
        private static readonly List<IBreakingChangeValidator> _validators = new List<IBreakingChangeValidator>
        {
            new HasOldNavigationViewValidator(),
            new HasPivotValidator(),
            new HasNoCoreProjectValidator(),
            new UsesVersionSevenOfMvvmToolkitValidator(),
        };

        public static ValidationResult Validate()
        {
            var projectMetadata = ProjectMetadataService.GetProjectMetadata(GenContext.ToolBox.Shell.Project.GetActiveProjectPath());
            var projectVersion = projectMetadata.WizardVersion.ToVersion();

            var validations = _validators
                                .Where(v => projectVersion <= v.BreakingVersion)
                                .Select(v => v.Validate()).ToList();

            return new ValidationResult
            {
                IsValid = validations.All(v => v.IsValid),
                ErrorMessages = validations.SelectMany(v => v.ErrorMessages).ToList(),
            };
        }

        private static Version ToVersion(this string stringVersion)
        {
            if (string.IsNullOrEmpty(stringVersion))
            {
                return new Version();
            }

            var projectVersion = stringVersion.TrimStart('v');
            return new Version(projectVersion);
        }
    }
}
