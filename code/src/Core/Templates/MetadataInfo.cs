// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core
{
    public class MetadataInfo
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Icon { get; set; }

        public string Summary { get; set; }

        public string Description { get; set; }

        public string Author { get; set; }

        public int Order { get; set; }

        public MetadataType MetadataType { get; set; }

        public string Licenses { get; set; }

        public IEnumerable<TemplateLicense> LicenseTerms { get; set; }

        public IEnumerable<string> Platforms { get; set; }

        public IEnumerable<string> Languages { get; set; }

        public IDictionary<string, object> Tags { get; set; }
    }
}
