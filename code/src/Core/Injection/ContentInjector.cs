using Microsoft.Templates.Core.Injection.Code;
using Microsoft.Templates.Core.Injection.References;
using Microsoft.Templates.Core.Injection.Xaml;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.Injection
{
    public abstract class ContentInjector
    {
        public abstract string Inject(string sourceContent);
    }

    public abstract class ContentInjector<TConfig> : ContentInjector
    {
        private Lazy<TConfig> _config;
        protected TConfig Config => _config.Value;

        public ContentInjector(string filePath)
        {
            _config = new Lazy<TConfig>(() => ReadConfig(filePath), true);
        }

        public ContentInjector(TConfig config)
        {
            _config = new Lazy<TConfig>(() => config, true);
        }

        private TConfig ReadConfig(string filePath)
        {
            //TODO: REVIEW FILE EXISTS
            var fileContent = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<TConfig>(fileContent);
        }
    }

    public class ContentInjectorFactory
    {
        public static ContentInjector Find(string filePath)
        {
            //TODO: EXTRACT THIS SOMEWHERE
            var originalFilePath = filePath.Replace(Path.GetExtension(filePath), string.Empty);

            if (Path.GetExtension(originalFilePath).Equals(".cs", StringComparison.OrdinalIgnoreCase))
            {
                return new CodeInjector(filePath);
            }
            else if (Path.GetExtension(originalFilePath).Equals(".xaml", StringComparison.OrdinalIgnoreCase))
            {
                return new XamlInjector(filePath);
            }
            else if (Path.GetFileName(originalFilePath).Equals("project.json", StringComparison.OrdinalIgnoreCase))
            {
                return new ReferencesInjector(filePath);
            }

            return null;
        }
    }
}
