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
    [Verb("list-versions", Hidden = false, HelpText = "List the available versions / templates packages for the specified environment.")]
    public class RemoteSourceListOptions : RemoteSourceCommonOptions
    {
        [Option('s', "summary", HelpText = "Shows summary info.", Default = true, SetName = "Summary")]
        public bool Summary { get; set; }

        [Option('d', "detailed", HelpText = "Shows detailed versions info.", Default= false, SetName = "Detailed")]
        public bool Detailed { get; set; }

        [Option('m', "main", HelpText = "Shows main versions info.", Default = false, SetName = "Main")]
        public bool Main { get; set; }
    }
}
