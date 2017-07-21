// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Templates.Core
{
    public abstract class Validator
    {
        public abstract ValidationResult Validate(string suggestedName);
    }

    public abstract class Validator<TConfig> : Validator
    {
        protected readonly TConfig _config;

        public Validator(TConfig config)
        {
            _config = config;
        }
    }
}
