// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using Microsoft.Templates.Core.Gen;

namespace Microsoft.Templates.UI.ViewModels.Common
{
    public class GenGroup
    {
        public string Name { get; set; }

        public IEnumerable<GenInfoGroup> GenGroups { get; set; }

        public GenGroup(string name, IEnumerable<GenInfo> genGroups)
        {
            Name = name;
            GenGroups = genGroups
                .GroupBy(WtsType)
                .Select(group => new GenInfoGroup(group.Key, group));
        }

        private string WtsType(GenInfo genInfo)
        {
            return genInfo.Template.TagsCollection.First(tag => tag.Key == "ts.type").Value;
        }
    }
}
