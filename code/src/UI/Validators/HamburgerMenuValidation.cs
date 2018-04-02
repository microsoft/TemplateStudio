// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Validation;
using Microsoft.Templates.UI.Services;

namespace Microsoft.Templates.UI.Validators
{
    public class HamburgerMenuValidation : BaseValidator
    {
        public override ValidationResult Validate()
        {
            var result = new ValidationResult();

            var hamgurguerVersion = new Version("1.7.0.0");
            var templatesVersion = GetVersion(GenContext.ToolBox.TemplatesVersion);
            var projectVersion = GetVersion(ProjectMetadataService.GetProjectMetadata().TemplatesVersion);

            // TODO - Missing  check navigationView/HamburguerMenu file
            if (projectVersion <= hamgurguerVersion && templatesVersion > hamgurguerVersion)
            {
                result.IsValid = false;
                result.ErrorMessages.Add("hamburguer menu breaking change detected");
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
    }
}
