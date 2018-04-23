// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Linq;
using LibGit2Sharp;
using Localization.Extensions;
using Localization.Options;

namespace Localization
{
    public class ValidateLocalizableExtractor
    {
        private RoutesManager _routesManager;

        public ValidateLocalizableExtractor(string originalExtractPath, string actualExtractPath)
        {
            _routesManager = new RoutesManager(originalExtractPath, actualExtractPath);
        }

        public bool HasVsixChanges()
        {
            var originalVsix = _routesManager.GetFileFromSource(Routes.VsixRootDirPath, Routes.VsixManifestFile).FullName;
            var actualVsix = _routesManager.GetFileFromDestination(Routes.VsixRootDirPath, Routes.VsixManifestFile).FullName;

            var (originalName, originalDescription) = RoutesExtensions.GetVsixValues(originalVsix);
            var (actualName, actualDescription) = RoutesExtensions.GetVsixValues(actualVsix);

            return !(originalName == actualName
                && originalDescription == actualDescription);
        }

        internal bool HasChanges(string projectTemplateFileNameValidateCS)
        {
            return true;
        }
    }
}
