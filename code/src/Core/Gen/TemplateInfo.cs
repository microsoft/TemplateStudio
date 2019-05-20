// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.Gen
{
    public class TemplateInfo
    {
        public string TemplateId { get; set; }

        public string Name { get; set; }

        public string DefaultName { get; set; }

        public string Description { get; set; }

        public string RichDescription { get; set; }

        public string Author { get; set; }

        public string Version { get; set; }

        public string Icon { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsHidden { get; set; }

        public string Group { get; set; }

        public bool IsGroupExclusiveSelection { get; set; }

        public int GenGroup { get; set; }

        public bool MultipleInstance { get; set; }

        public bool ItemNameEditable { get; set; }

        public IEnumerable<TemplateLicense> Licenses { get; set; } = new List<TemplateLicense>();

        public IEnumerable<TemplateInfo> Dependencies { get; set; } = new List<TemplateInfo>();

        public TemplateType TemplateType { get; set; }

        public FeatureType FeatureType { get; set; }

        public bool RightClickEnabled { get; set; }
    }
}
