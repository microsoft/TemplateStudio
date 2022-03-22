// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Templates.Core.Gen
{
    public class SupportedFramework
    {
        public string Name { get; internal set; }

        public FrameworkTypes Type { get; internal set; }

        public SupportedFramework(string name, FrameworkTypes type)
        {
            Name = name;
            Type = type;
        }
    }
}
