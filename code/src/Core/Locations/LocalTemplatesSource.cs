// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Helpers;

namespace Microsoft.Templates.Core.Locations
{
    public class LocalTemplatesSource : TemplatesSource
    {
        public static readonly TemplatesPackageInfo VersionZero = new TemplatesPackageInfo()
        {
            Name = "LocalTemplates_v0.0.0.0",
            LocalPath = $@"..\..\..\..\..\{TemplatesFolderName}",
            Bytes = 1024,
            WizardVersions = new List<Version>() { new Version(0, 0) },
            Date = DateTime.Now,
        };

        protected virtual string Origin => Path.Combine(Path.GetFullPath(InstalledPackagePath), TemplatesFolderName);

        private string _id;

        public override string InstalledPackagePath { get; }

        public override string Language { get => string.Empty; }

        public override string Platform { get => string.Empty; }

        private List<TemplatesPackageInfo> availablePackages = new List<TemplatesPackageInfo>();

        protected override bool VerifyPackageSignatures => false;

        public override string Id { get => _id; }

        public LocalTemplatesSource(string installedPackagePath)
            : this(installedPackagePath, GenContext.GetWizardVersionFromAssembly().ToString())
        {
            _id = Configuration.Current.Environment + GetAgentName();
        }

        public LocalTemplatesSource(string installedPackagePath, string id)
            : this(installedPackagePath, GenContext.GetWizardVersionFromAssembly().ToString(), id)
        {
            _id = id + GetAgentName();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Justification = "Used to override the default value for this property in the local (test) source")]
        public LocalTemplatesSource(string installedPackagePath, string templatesVersion, string id)
        {
            if (string.IsNullOrEmpty(_id))
            {
                _id = Configuration.Current.Environment + GetAgentName();
            }

            if (string.IsNullOrEmpty(installedPackagePath))
            {
                InstalledPackagePath = $@"..\..\..\..\..";
            }
            else
            {
                InstalledPackagePath = installedPackagePath;
            }

            availablePackages.Add(VersionZero);
            Version.TryParse(templatesVersion, out Version v);
            if (!v.IsZero())
            {
                var package = new TemplatesPackageInfo()
                {
                    Name = $"LocalTemplates_v{v.ToString()}",
                    LocalPath = $@"{InstalledPackagePath}\{TemplatesFolderName}",
                    WizardVersions = new List<Version>() { v },
                    Bytes = 1024,
                    Date = DateTime.Now,
                };

                availablePackages.Add(package);
            }
        }

        public override async Task LoadConfigAsync(CancellationToken ct)
        {
            await Task.Run(() =>
            Config = new TemplatesSourceConfig()
            {
                Versions = availablePackages,
                Latest = availablePackages.OrderByDescending(p => p.Version).FirstOrDefault(),
            });
        }

        public override async Task<TemplatesContentInfo> GetContentAsync(TemplatesPackageInfo packageInfo, string workingFolder, CancellationToken ct)
        {
            string targetFolder = Path.Combine(workingFolder, packageInfo.Version.ToString());

            if (Directory.Exists(workingFolder))
            {
                Fs.SafeDeleteDirectory(workingFolder);
            }

            await Task.Run(() =>
            {
                JunctionNativeMethods.CreateJunction(Origin, targetFolder, true);
            });

            return new TemplatesContentInfo()
            {
                Version = packageInfo.Version,
                Path = targetFolder,
                Date = packageInfo.Date,
            };
        }

        public override async Task AcquireAsync(TemplatesPackageInfo packageInfo, CancellationToken ct)
        {
            await Task.Run(() =>
            {
                packageInfo.LocalPath = Origin;
            });
        }

        protected static string GetAgentName()
        {
            // If running tests in VSTS concurrently in different agents avoids the collison in templates folders
            string agentName = Environment.GetEnvironmentVariable("AGENT_NAME");
            if (string.IsNullOrEmpty(agentName))
            {
                return string.Empty;
            }
            else
            {
                return $"-{agentName}";
            }
        }
    }
}
