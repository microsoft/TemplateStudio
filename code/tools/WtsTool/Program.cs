// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using WtsTool.CommandOptions;

namespace WtsTool
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Out.WriteHeader("Wts Tool v1.0.0.0");

            Parser.Default.ParseArguments<PackageOptions, ListOptions, DownloadOptions, PublishOptions>(args)
            .MapResult(
                (PackageOptions packOpts) => { return RunCommand.Package(packOpts, Console.Out, Console.Error); },
                (ListOptions listOpts) => { return RunCommand.List(listOpts, Console.Out, Console.Error); },
                (DownloadOptions downloadOpts) => { return RunCommand.Download(downloadOpts, Console.Out, Console.Error); },
                (PublishOptions publishOpts) => { return RunCommand.Publish(publishOpts, Console.Out, Console.Error); },
                errs => 1);

            Console.Out.WriteFooter();
        }
    }
}
