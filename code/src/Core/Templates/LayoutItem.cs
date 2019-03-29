// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Newtonsoft.Json;

namespace Microsoft.Templates.Core
{
    public class LayoutItem
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("templateGroupIdentity")]
        public string TemplateGroupIdentity { get; set; }

        [JsonProperty("readonly")]
        public bool Readonly { get; set; }

        [JsonProperty("projecttype")]
        public string ProjectType { get; set; }
    }
}
