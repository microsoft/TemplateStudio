// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Microsoft.Templates.Core.Naming
{
    public class ValidationResult
    {
        public bool IsValid { get; set; } = true;

        public List<ValidationError> Errors { get; set; } = new List<ValidationError>();
    }
}
