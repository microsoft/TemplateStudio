// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace WtsTool.CommandOptions
{
    [Verb("download", Hidden = false, HelpText = "Download a templates package from the specified environment.")]
    public class RemoteSourceDownloadOptions : RemoteSourceCommonOptions
    {
        [Option('l', "latest", Required = true, HelpText = "Download the latest available version of templates package for the specified environment.", Default = "true", SetName = "Version")]
        public bool Latest { get; set; }

        [Option('v', "version", Required = true, HelpText = "Download a concrete version of templates package for the specified environment Downloads the most recent matching version.", SetName = "Version")]
        public string Version { get; set; }
    }
}
