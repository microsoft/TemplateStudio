// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace Microsoft.Templates.UI.Services
{
    public class DotNetValidator : IRequirementValidator
    {
        public const string Id = "dotnet";
        public const string DisplayName = "[.NET Core Runtime](https://dotnet.microsoft.com/download/dotnet-core/3.1)";
        private const string WindowsAppRuntimeName = "Microsoft.WindowsDesktop.App 3.1";

        public bool IsVersionInstalled(Version version)
        {
            using (var process = new Process())
            {
                process.StartInfo = new ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments = "--list-runtimes",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                };

                try
                {
                    process.Start();

                    var result = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    var runtimeVersions = result.Split(new[] { Environment.NewLine }, StringSplitOptions.None)
                                          .Where(v => v.StartsWith(WindowsAppRuntimeName, StringComparison.OrdinalIgnoreCase))
                                          .Select(r => new Version(r.Split(' ')[1]));

                    if (runtimeVersions.Any(r => r.CompareTo(version) >= 0))
                    {
                        return true;
                    }
                }
                catch (Win32Exception)
                {
                    return false;
                }
            }

            return false;
        }
    }
}
