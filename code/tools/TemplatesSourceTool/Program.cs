using CommandLine;
using System;
using TemplatesSourceTool.CommandOptions;

namespace TemplatesSourceTool
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<PackageOptions>(args)
                    .MapResult(
                        (PackageOptions opts) => { Console.WriteLine("PackageOptions"); return 0 ; }, errs => 1
                     );
        }
    }
}
