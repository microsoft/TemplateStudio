// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Templates.Core.Gen;
using System;

namespace Microsoft.Templates.UI.Services
{
    public static class BreakingChangesValidatorService
    {
        public static bool HasNavigationViewBreakingChange()
        {
            var hamgurguerVersion = new Version("1.7.0.0");
            var templatesVersion = GenContext.ToolBox.TemplatesVersion.ToVersion();
            var projectVersion = ProjectMetadataService.GetProjectMetadata().TemplatesVersion.ToVersion();

            // TODO - Missing  check navigationView/HamburguerMenu file
            return projectVersion <= hamgurguerVersion && templatesVersion > hamgurguerVersion;
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
