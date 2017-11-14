using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplatesSourceTool.CommandOptions;

namespace TemplatesSourceTool
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<PackageOptions>(args)
                .MapResult(
                    (PackageOptions opts) => { Console.WriteLine($"Action: {opts.Action}; Target: {opts.Source}"); return 0; }, errs => 1
                 );
        }
    }
}
