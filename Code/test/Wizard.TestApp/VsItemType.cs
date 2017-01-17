using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Microsoft.Templates.Wizard.TestApp
{
    public enum VsItemType
    {
        Compiled,
        CompiledWithDependant,
        XamlPage,
        Content
    }

    public static class VsItemTypeExtensions
    {
        public static XElement GetXmlDefinition(this VsItemType itemType, string includePath)
        {
            if (itemType == VsItemType.Compiled)
            {
                return GetCompileXElement(includePath, false);
            }
            if (itemType == VsItemType.CompiledWithDependant)
            {
                return GetCompileXElement(includePath, true);
            }
            if (itemType == VsItemType.XamlPage)
            {
                return GetXamlPageXElement(includePath);
            }
            if (itemType == VsItemType.Content)
            {
                return GetContentXElement(includePath);
            }
            return null;
        }

        private static XElement GetCompileXElement(string includePath, bool dependentUpon)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"<Compile Include=\"{includePath}\"");
            if (dependentUpon)
            {
                string dependency = Path.GetFileNameWithoutExtension(includePath);
                sb.AppendLine(">");
                sb.AppendLine($"<DependentUpon>{dependency}</DependentUpon>");
                sb.AppendLine("</Compile>");
            }
            else
            {
                sb.AppendLine(" />");
            }

            StringReader sr = new StringReader(sb.ToString());
            XElement itemElement = XElement.Load(sr);
            return itemElement;
        }

        private static XElement GetXamlPageXElement(string includePath)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"<Page Include=\"{includePath}\"");
            sb.AppendLine(">");
            sb.AppendLine($"<Generator>MSBuild:Compile</Generator>");
            sb.AppendLine($"<SubType>Designer</SubType>");
            sb.AppendLine("</Page>");

            StringReader sr = new StringReader(sb.ToString());
            XElement itemElement = XElement.Load(sr);
            return itemElement;
        }

        private static XElement GetContentXElement(string includePath)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"<Content Include=\"{includePath}\" />");
            StringReader sr = new StringReader(sb.ToString());
            XElement itemElement = XElement.Load(sr);
            return itemElement;
        }
    }
}
