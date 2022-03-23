// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Templates.Core.Naming
{
    public class ProjectNameValidationConfig
    {
        public RegExConfig[] Regexs { get; set; }

        public string[] ReservedNames { get; set; }

        public bool ValidateExistingNames { get; set; }

        public bool ValidateEmptyNames { get; set; }
    }
}
