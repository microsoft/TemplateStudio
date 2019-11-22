// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Naming;
using Microsoft.Templates.UI.Controls;
using Microsoft.Templates.UI.Resources;

namespace Microsoft.Templates.UI.Extensions
{
    public static class ValidationResultExtensions
    {
        public static Notification GetNotification(this ValidationResult validationResult)
        {
            switch (validationResult.ErrorType)
            {
                case ValidationErrorType.EmptyName:
                    return Notification.Error(StringRes.NotificationValidationError_Empty, ErrorCategory.NamingValidation, CategoriesToOverride);
                case ValidationErrorType.AlreadyExists:
                    return Notification.Error(StringRes.NotificationValidationError_AlreadyExists, ErrorCategory.NamingValidation, CategoriesToOverride);
                case ValidationErrorType.Regex:
                    switch (validationResult.ValidatorName)
                    {
                        case "badFormat":
                            return Notification.Error(StringRes.NotificationValidationError_BadFormat, ErrorCategory.NamingValidation, CategoriesToOverride);
                        case "itemEndsWithPage":
                            return Notification.Error(string.Format(StringRes.NotificationValidationError_PageSuffix, Configuration.Current.GitHubDocsUrl), ErrorCategory.NamingValidation, CategoriesToOverride);
                        default:
                            return Notification.Error(string.Format(StringRes.NotificationValidationError_Regex, validationResult.ValidatorName));
                    }

                case ValidationErrorType.ReservedName:
                    return Notification.Error(StringRes.NotificationValidationError_ReservedName, ErrorCategory.NamingValidation, CategoriesToOverride);
                default:
                    return null;
            }
        }

        private static IEnumerable<Category> CategoriesToOverride
        {
            get
            {
                yield return Category.RemoveTemplateValidation;
            }
        }
    }
}
