// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Validation;
using Microsoft.Templates.SharedResources;

namespace Microsoft.Templates.UI.Validators
{
    public class UsesVersionSevenOfMvvmToolkitValidator : IBreakingChangeValidator
    {
        public Version BreakingVersion { get; }

        public UsesVersionSevenOfMvvmToolkitValidator()
        {
            // This is last version which used MVVM Toolkit 7.x
            var version = Core.Configuration.Current.BreakingChangesVersions.FirstOrDefault(c => c.Name == "UsesVersionSevenOfMvvmToolkit")?.BreakingVersion;
            BreakingVersion = version ?? new Version();
        }

        public ValidationResult Validate()
        {
            var result = new ValidationResult();

            if (UsesVersionSeven())
            {
                var message = new ValidationMessage
                {
                    Message = Resources.ValidatorUsesMvvmToolkitSevenMessage,
                    Url = string.Format(Resources.ValidatorUsesMvvmToolkitSevenLink, Core.Configuration.Current.GitHubDocsUrl),
                    HyperLinkMessage = Resources.ValidatorUsesMvvmToolkitSevenLinkMessage,
                };

                result.IsValid = false;
                result.ErrorMessages.Add(message);
            }

            return result;
        }

        private bool UsesVersionSeven()
        {
            try
            {
                var projectName = GenContext.ToolBox.Shell.Project.GetActiveProjectName();
                var projectFolder = GenContext.ToolBox.Shell.Project.GetActiveProjectPath();
                var projectPath = Path.Combine(projectFolder, $"{projectName}.csproj");

                var xdoc = XDocument.Load(projectPath);

                foreach (var item in xdoc.Descendants().Where(d => d.Name == "PackageReference"))
                {
                    var includeAttribute = item.Attribute("Include");

                    if (includeAttribute != null && includeAttribute.Value == "CommunityToolkit.Mvvm")
                    {
                        var versionAttribute = item.Attribute("Version");
                        if (versionAttribute != null)
                        {
                            return versionAttribute.Value.StartsWith("7");
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                System.Diagnostics.Debug.WriteLine(exc);
            }

            // If in doubt, assume this isn't a breaking upgrade
            return false;
        }
    }
}
