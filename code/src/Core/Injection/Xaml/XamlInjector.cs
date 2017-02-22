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
                    if (!string.IsNullOrEmpty(element.content))
                    {
                        var newElement = XElement.Parse(element.content);

                        container.Add(newElement);
                        source.CopyNamespaces(newElement); 
                    }

                    if (element.attributes != null)
                    {
                        foreach (var attr in element.attributes)
                        {
                            container.Add(new XAttribute(attr.name, attr.value));
                        }
                    }
                }
            }
            return source.ToString(SaveOptions.OmitDuplicateNamespaces);
        }
    }
}
