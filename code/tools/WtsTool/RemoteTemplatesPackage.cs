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
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using WtsTool.CommandOptions;

namespace WtsTool
{
    public class RemoteTemplatesPackage
    {
        public string Name { get; set; }

        public Uri StorageUri { get; set; }

        public DateTime Date { get; set; }

        public long Bytes { get; set; }

        public Version Version { get; set; }

        [JsonIgnore]
        public string MainVersion { get => Version != null ? $"{Version.Major.ToString()}.{Version.Minor.ToString()}" : "NoVersion"; }
    }
}
