// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Xunit;

namespace Microsoft.Templates.Test
{
    public abstract class BaseProjectFileTests
    {
        public abstract void CheckTemplateReferences();

        public void CheckTemplateReferencesInternal(string projFilePath)
        {
            var fileContents = File.ReadAllText(projFilePath);

            var xDoc = new XmlDocument();
            xDoc.LoadXml(fileContents);

            var errors = new List<string>();

            int itemGroups = 0;

#pragma warning disable CS8602 // Dereference of a possibly null reference. - Want this to throw as it's a definite fail
            foreach (var item in xDoc.LastChild.ChildNodes)
            {
                System.Diagnostics.Debug.WriteLine(item);

                if (item is XmlElement xe && xe.Name == "ItemGroup")
                {
                    itemGroups++;

                    // if (xe.FirstChild.Attributes)
                    foreach (var node in xe.ChildNodes)
                    {
                        if (node is XmlElement ele)
                        {
                            if (ele.Name.EndsWith("Reference", System.StringComparison.InvariantCultureIgnoreCase))
                            {
                                break;
                            }

                            var incAttr = ele.Attributes["Include"];

                            if (incAttr == null)
                            {
                                errors.Add($"Element missing expected `Include` attribute: {ele.OuterXml}");
                            }
                            else
                            {
                                if (ele.Attributes.Count > 1)
                                {
                                    errors.Add($"Element has too many attributes: {ele.OuterXml}");
                                }

                                if (incAttr.Value.StartsWith("Templates\\", System.StringComparison.InvariantCulture))
                                {
                                    if (ele.Name != "Content")
                                    {
                                        errors.Add($"Element should be a `Content`: {ele.OuterXml}");
                                    }
                                    else
                                    {
                                        if (ele.ChildNodes.Count != 1
                                         || ele.FirstChild.Name != "IncludeInVSIX"
                                         || ele.FirstChild.InnerText != "true")
                                        {
                                            errors.Add($"Incorrect child elements for: {ele.OuterXml}");
                                        }
                                    }

                                    // check `Include` starting "Template"
                                    // check those children are "Include" elements
                                    // check the only child is "<IncludeInVSIX>true</IncludeInVSIX>"
                                }
                            }
                        }
                    }
                }
            }
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            System.Diagnostics.Debug.WriteLine($"ItemGroups found = {itemGroups}");

            // Assert.Equal(5, itemGroups);
            Assert.True(!errors.Any(), string.Join(Environment.NewLine, errors.ToArray()));
        }
    }
}
