// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using Microsoft.Templates.Core.Gen;

namespace Microsoft.Templates.UI.ViewModels.Common
{
    public class GenInfoGroup
    {
        public string Name { get; set; }

        public IEnumerable<GenInfoComposition> Compositions { get; set; }

        public bool IsCompositionGroup { get; set; }

        public GenInfoGroup(string name, IEnumerable<GenInfo> compositions)
        {
            Name = name;
            Compositions = compositions.Select(item => new GenInfoComposition(item));
            IsCompositionGroup = name == "composition";
        }
    }
}
