// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;

using Microsoft.Templates.Core.Gen;

namespace Microsoft.Templates.Core.Diagnostics
{
    public class GenItemsTelemetryData
    {
        public GenItemsTelemetryData(IEnumerable<GenInfo> genItems)
        {
            PagesCount = genItems.Count(t => t.Template.GetTemplateType() == TemplateType.Page);
            FeaturesCount = genItems.Count(t => t.Template.GetTemplateType() == TemplateType.Feature);
            ServicesCount = genItems.Count(t => t.Template.GetTemplateType() == TemplateType.Service);
            TestingCount = genItems.Count(t => t.Template.GetTemplateType() == TemplateType.Testing);
            PageIdentities = string.Join(",", genItems.Where(t => t.Template.GetTemplateType() == TemplateType.Page).Select(t => t.Template.Identity));
            FeatureIdentities = string.Join(",", genItems.Where(t => t.Template.GetTemplateType() == TemplateType.Feature).Select(t => t.Template.Identity));
            ServiceIdentities = string.Join(",", genItems.Where(t => t.Template.GetTemplateType() == TemplateType.Service).Select(t => t.Template.Identity));
            TestingIdentities = string.Join(",", genItems.Where(t => t.Template.GetTemplateType() == TemplateType.Testing).Select(t => t.Template.Identity));
        }

        public int? PagesCount { get; set; }

        public int? FeaturesCount { get; set; }

        public int? ServicesCount { get; set; }

        public int? TestingCount { get; set; }

        public string PageIdentities { get; set; }

        public string FeatureIdentities { get; set; }

        public string ServiceIdentities { get; set; }

        public string TestingIdentities { get; set; }
    }
}
