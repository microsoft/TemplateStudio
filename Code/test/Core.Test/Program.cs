using Microsoft.TemplateEngine.Abstractions.Mount;
using Microsoft.TemplateEngine.Edge.Settings;
using Microsoft.Templates.Core.Locations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var repo = new TemplatesRepository(new TestTemplatesLocation());
            repo.Sync();

            var result = repo.GetAll();

            //SettingsLoader.TryGetMountPointInfo(result.First().ConfigMountPointId, out MountPointInfo info);

            //SettingsLoader.TryGetFileFromIdAndPath(result.First().ConfigMountPointId, result.First().ConfigPlace, out IFile f);
            //var configDir = Path.GetFullPath(f.Parent.FullPath);

            //var c = SettingsLoader.Components;

            //var m = SettingsLoader.MountPoints;

            //var tConfig = info.Place + result.First().ConfigPlace.Replace("/", @"\");
            //var tDir = Path.GetDirectoryName(tConfig);


            result.ToList().ForEach(t => Console.WriteLine($"{t.Name} ({t.GetTemplateType()})"));
        }
    }
}
