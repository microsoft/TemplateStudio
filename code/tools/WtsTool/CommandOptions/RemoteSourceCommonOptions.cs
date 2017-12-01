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
    public class RemoteSourceCommonOptions : CommonOptions
    {
        [Option('a', "storage-account", HelpText = "Storage account for remote templates source operation.", Default = "wtsrepository")]
        public string StorageAccount { get; set; }

        [Option('e', "env", HelpText = "Environment. Valid values: Pro, Pre, Dev or Test", Default = EnvEnum.Dev)]
        public EnvEnum Env { get; set; }
    }
}
