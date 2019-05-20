// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Resources;

namespace Localization
{
    public class ResxItem
    {
        public string Name { get; set; }

        public string Text { get; set; }

        public string Comment { get; set; }

        public ResXDataNode ToNode()
        {
            return new ResXDataNode(Name, Text)
            {
                Comment = Comment,
            };
        }
    }
}
