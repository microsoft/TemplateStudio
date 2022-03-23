// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Templates.Core.Naming
{
    public class ValidationError
    {
        public ValidationErrorType ErrorType { get; set; }

        public string ValidatorName { get; set; }
    }
}
