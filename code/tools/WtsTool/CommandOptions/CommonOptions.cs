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
    public class CommonOptions
    {
        [Option("verbose", HelpText = "Show verbose error info.", Default = false)]
        public bool Verbose { get; set; }
    }
}
