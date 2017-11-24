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
    [Verb("publish", Hidden = false, HelpText = "Publish a templates package to the specified environment.")]
    public class RemoteSourcePublishOptions : RemoteSourceCommonOptions
    {
        [Option('k', "storage-key", Required = true, HelpText = "Storage Account key to be used for package publishing.")]
        public string AccountKey { get; set; }

        [Option('f', "file", Required = true, HelpText = "Mstx file to publish.", SetName = "Package")]
        public string File { get; set; }

        [Option('v', "version", Required = true, HelpText = "Version number for the templates package to be published.", SetName = "Package")]
        public string Version { get; set; }

        [Option('c', "config", Required = true, HelpText = "Publish an updated configuration file to the CDN.", Default = false, SetName = "Config")]
        public bool Config { get; set; }
    }
}
