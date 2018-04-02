// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;

namespace Microsoft.Templates.Core.Validation
{
    public abstract class BaseValidator : IValidator
    {
        public abstract ValidationResult Validate();

        public virtual bool IsValid() => !Validate().ErrorMessages.Any();
    }
}
