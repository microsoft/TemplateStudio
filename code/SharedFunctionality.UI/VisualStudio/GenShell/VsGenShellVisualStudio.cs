// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EnvDTE;
using Microsoft.Build.Utilities;
using Microsoft.Templates.Core.Gen.Shell;
using Microsoft.Templates.UI.Threading;
using Microsoft.VisualStudio.Setup.Configuration;

namespace Microsoft.Templates.UI.VisualStudio.GenShell
{
    public class VsGenShellVisualStudio : IGenShellVisualStudio
    {
        private readonly VsShellService _vsShellService;

        private readonly List<string> installedPackageIds = new List<string>();

        private string _vsVersionInstance = string.Empty;

        private string _vsProductVersion = string.Empty;

        public VsGenShellVisualStudio(VsShellService vsShellService)
        {
            _vsShellService = vsShellService;
        }

        public string GetVsVersion()
        {
            if (string.IsNullOrEmpty(_vsProductVersion))
            {
                ISetupConfiguration configuration = new SetupConfiguration() as ISetupConfiguration;
                ISetupInstance instance = configuration.GetInstanceForCurrentProcess();
                _vsProductVersion = instance.GetInstallationVersion();
            }

            return _vsProductVersion;
        }

        public string GetVsVersionAndInstance()
        {
            if (string.IsNullOrEmpty(_vsVersionInstance))
            {
                ISetupConfiguration configuration = new SetupConfiguration() as ISetupConfiguration;
                ISetupInstance instance = configuration.GetInstanceForCurrentProcess();
                string version = instance.GetInstallationVersion();
                string instanceId = instance.GetInstanceId();
                _vsVersionInstance = $"{version}-{instanceId}";
            }

            return _vsVersionInstance;
        }

        public bool IsBuildInProgress()
        {
            return SafeThreading.JoinableTaskFactory.Run(async () =>
            {
                return await IsBuildInProgressAsync();
            });
        }

        public async Task<bool> IsBuildInProgressAsync()
        {
            await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
            var dte = await _vsShellService.GetDteAsync();
            return dte.Solution.SolutionBuild.BuildState == vsBuildState.vsBuildStateInProgress;
        }

        public bool IsDebuggerEnabled()
        {
            return SafeThreading.JoinableTaskFactory.Run(async () =>
            {
                return await IsDebuggerEnabledAsync();
            });
        }

        public async Task<bool> IsDebuggerEnabledAsync()
        {
            await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
            var dte = await _vsShellService.GetDteAsync();
            return dte.Debugger.CurrentMode != dbgDebugMode.dbgDesignMode;
        }

        public bool IsSdkInstalled(string version)
        {
            var sdks = ToolLocationHelper.GetTargetPlatformSdks();

            foreach (var sdk in sdks)
            {
                var versions = ToolLocationHelper.GetPlatformsForSDK(sdk.TargetPlatformIdentifier, sdk.TargetPlatformVersion);
                if (versions.Any(v => v.Contains(version)))
                {
                    return true;
                }
            }

            return false;
        }

        public List<string> GetInstalledPackageIds()
        {
            if (!installedPackageIds.Any())
            {
                var packages = _vsShellService.GetSetupInstance().GetPackages();
                foreach (var package in packages)
                {
                    installedPackageIds.Add(package.GetId());
                }
            }

            return installedPackageIds;
        }
    }
}
