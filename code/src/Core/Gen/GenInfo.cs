// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

using Microsoft.TemplateEngine.Abstractions;

namespace Microsoft.Templates.Core.Gen
{
    public class GenInfo
    {
        public string Name { get; set; }
        public ITemplateInfo Template { get; set; }
        public Dictionary<string, string> Parameters { get; } = new Dictionary<string, string>();

        public string GetUserName()
        {
            if (Parameters.ContainsKey(GenParams.Username))
            {
                return Parameters[GenParams.Username];
            }

            return string.Empty;
        }

        public override string ToString()
        {
            return $"{Name}, {Template?.Name}";
        }
    }
}
