// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace TemplateValidator
{
    public class PostActionInfo
    {
        public string Description { get; set; }

        public string ActionId { get; set; }

        public List<string> ManualInstructions { get; set; }

        public Dictionary<string, string> Args { get; set; }

        public bool ContinueOnError { get; set; }
    }
}
