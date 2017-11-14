using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplatesSourceTool.CommandOptions;

namespace TemplatesSourceTool
{
    public class RunCommand
    {
        public void Package(PackageOptions options, StreamWriter output, StreamWriter error)
        {
            switch (options.Action)
            {
                case PackageAction.Create:
                    PackageWorker.Create(options.Source, options.CertThumbprint, output, error);
                    break;
                case PackageAction.Extract:
                    PackageWorker.Extract(options.Source, output, error);
                    break;
                case PackageAction.Info:
                    PackageWorker.GetInfo(options.Source, output, error);
                    break;
                default:
                    error.WriteLine($"Unexpected action '{options.Action.ToString()}'");
                    break;
            }
        }
    }
}
