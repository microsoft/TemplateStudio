// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Templates.UI.Resources;

namespace Microsoft.Templates.UI.Services
{
    public class RequiredVersionService
    {
        private readonly Dictionary<string, bool> installedVersions = new Dictionary<string, bool>();

        private static readonly Lazy<RequiredVersionService> _instance = new Lazy<RequiredVersionService>(() => new RequiredVersionService());

        public static RequiredVersionService Instance => _instance.Value;

        private RequiredVersionService()
        {
        }

        public static RequiredVersionInfo GetVersionInfo(string requirement)
        {
            var requirementInfo = requirement.Split(',');

            var requirementType = ParseRequirementType(requirementInfo[0]);
            var requirementVersion = Regex.Match(requirementInfo[1], @"\d+(\.\d+)+").Value;

            return new RequiredVersionInfo()
            {
                Id = requirement,
                RequirementType = requirementType,
                Version = new Version(requirementVersion),
            };
        }

        public bool IsVersionInstalled(RequiredVersionInfo requirement)
        {
            if (installedVersions.ContainsKey(requirement.Id))
            {
                return installedVersions[requirement.Id];
            }

            IRequirementValidator validator = GetRequirementValidator(requirement.RequirementType);
            var result = validator.IsVersionInstalled(requirement.Version);

            installedVersions.Add(requirement.Id, result);
            return result;
        }

        public static string GetRequirementDisplayName(RequiredVersionInfo versionInfo)
        {
            switch (versionInfo.RequirementType)
            {
                case RequirementType.WindowsSDK:
                    return $"{WindowsSDKValidator.DisplayName}  {versionInfo.Version}";

                case RequirementType.DotNetRuntime:
                    return $"{DotNetValidator.DisplayName} {versionInfo.Version}";
            }

            return string.Empty;
        }

        private static RequirementType ParseRequirementType(string requirementType)
        {
            if (requirementType.ToLower() == WindowsSDKValidator.Id.ToLower())
            {
                return RequirementType.WindowsSDK;
            }

            if (requirementType.ToLower() == DotNetValidator.Id.ToLower())
            {
                return RequirementType.DotNetRuntime;
            }

            return RequirementType.Unknown;
        }

        private static IRequirementValidator GetRequirementValidator(RequirementType requirementType)
        {
            switch (requirementType)
            {
                case RequirementType.WindowsSDK:
                    return new WindowsSDKValidator();
                case RequirementType.DotNetRuntime:
                    return new DotNetValidator();
                default:
                    throw new InvalidDataException(string.Format(StringRes.ErrorInvalidRequirementType, requirementType));
            }
        }
    }
}
