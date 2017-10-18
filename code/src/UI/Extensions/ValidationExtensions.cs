// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Templates.Core;
using Microsoft.Templates.UI.Resources;

namespace Microsoft.Templates.UI.Extensions
{
    public static class ValidationExtensions
    {
        public static string GetResourceString(this ValidationErrorType validationErrorType)
        {
            var resourceString = StringRes.ResourceManager.GetString($"ValidationError_{validationErrorType}");
            if (string.IsNullOrWhiteSpace(resourceString))
            {
                resourceString = StringRes.UndefinedErrorString;
            }

            return resourceString;
        }
    }
}
