using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Microsoft.Templates.Core.Injection.Xaml
{
    public class XamlInjector : ContentInjector<XamlInjectorConfig>
    {
        public XamlInjector(string filePath) : base(filePath)
        {
        }

        public XamlInjector(XamlInjectorConfig config) : base(config)
        {
        }

        public override string Inject(string sourceContent)
        {
            if (Config == null || string.IsNullOrEmpty(sourceContent))
            {
                //TODO: HOW HANDLE??
                return null;
            }
            var source = XElement.Parse(sourceContent);

            foreach (var element in Config.elements)
            {
                var container = source.Select(element.path);
                if (container != null)
                {
                    var newElement = XElement.Parse(element.content);

                    container.Add(newElement);
                    source.CopyNamespaces(newElement);
                }
            }
            return source.ToString(SaveOptions.OmitDuplicateNamespaces);
        }
    }
}
