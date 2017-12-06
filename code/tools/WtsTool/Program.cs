// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            var chrono = Stopwatch.StartNew();

            Console.Out.WriteHeader("Wts Tool v1.0.0.0");

            Parser.Default.ParseArguments<PackageOptions, RemoteSourceListOptions, RemoteSourceDownloadOptions, RemoteSourcePublishOptions>(args)
            .WithParsed<CommonOptions>(opts => TextWriterExtensions.Verbose = opts.Verbose)
            .MapResult(
                (PackageOptions packOpts) => { return RunCommand.Package(packOpts, Console.Out, Console.Error); },
                (RemoteSourceListOptions listOpts) => { return RunCommand.List(listOpts, Console.Out, Console.Error); },
                (RemoteSourceDownloadOptions downloadOpts) => { return RunCommand.Download(downloadOpts, Console.Out, Console.Error); },
                (RemoteSourcePublishOptions publishOpts) => { return RunCommand.Publish(publishOpts, Console.Out, Console.Error); },
                errs => 1);

            Console.Out.WriteFooter($"{chrono.Elapsed.TotalSeconds} sec.");

            ConsoleHelper.ShowCursor();
        }
    }
}
