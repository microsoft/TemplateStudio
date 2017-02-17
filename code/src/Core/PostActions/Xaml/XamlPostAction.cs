using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Microsoft.Templates.Core.PostActions.Xaml
{
    public class XamlPostAction
    {
        public string Execute(XamlPostActionConfig config, string sourceContent)
        {
            if (config == null || string.IsNullOrEmpty(sourceContent))
            {
                //TODO: HOW HANDLE??
                return null;
            }
            var source = XElement.Parse(sourceContent);

            foreach (var element in config.elements)
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

            //TODO: HANDLE ATTRIBUTES FORMAT && ATTRIBUTES ORDER
            //return ToStringFormatted(source);
        }

        //public static string ToStringFormatted(XElement xml)
        //{
        //    XmlWriterSettings settings = new XmlWriterSettings();

        //    settings.Indent = true;
        //    //settings.NewLineOnAttributes = true;
        //    settings.OmitXmlDeclaration = true;
        //    settings.NamespaceHandling = NamespaceHandling.OmitDuplicates;
        //    settings.NewLineHandling = NewLineHandling.Entitize;

        //    StringBuilder result = new StringBuilder();
        //    using (XmlWriter writer = XmlWriter.Create(result, settings))
        //    {
        //        xml.WriteTo(writer);
        //    }
        //    return result.ToString();
        //}
    }
}
