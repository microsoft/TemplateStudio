// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Microsoft.Templates.Test.Artifacts.MSBuild
{
    public enum VsItemType
    {
        Compiled,
        CompiledWithDependant,
        XamlPage,
        Resource,
        Content
    }

    public static class VsItemTypeExtensions
    {
        public static XElement GetXmlDefinition(this VsItemType itemType, string includePath)
        {
            switch (itemType)
            {
                case VsItemType.Compiled:
                    return GetCompileXElement(includePath, false);
                case VsItemType.CompiledWithDependant:
                    return GetCompileXElement(includePath, true);
                case VsItemType.XamlPage:
                    return GetXamlPageXElement(includePath);
                case VsItemType.Resource:
                    return GetResourceXElement(includePath);
                case VsItemType.Content:
                    return GetContentXElement(includePath);
                default:
                    return null;
            }
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

        private static XElement GetResourceXElement(string includePath)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"<PRIResource Include=\"{includePath}\" />");

            XElement itemElement = XElement.Parse(sb.ToString());
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

