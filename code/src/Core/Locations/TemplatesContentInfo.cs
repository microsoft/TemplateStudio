// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Packaging;
using Microsoft.Templates.Core.Resources;
using Newtonsoft.Json;

namespace Microsoft.Templates.Core.Locations
{
    public class TemplatesContentInfo
    {
        public string Path { get; set; }

        public DateTime Date { get; set; }

        public Version Version { get; set; }

        public string MainVersion { get => !Version.IsNull() ? $"{Version.Major.ToString()}.{Version.Minor.ToString()}" : "NoVersion"; }
    }
}
