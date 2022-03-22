// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Templates.Core.Locations
{
    public class TemplatesContentInfo
    {
        public string Path { get; set; }

        public DateTime Date { get; set; }

        public string Version { get; set; }

        public string MainVersion { get => Version ?? "NoVersion"; }
    }
}
