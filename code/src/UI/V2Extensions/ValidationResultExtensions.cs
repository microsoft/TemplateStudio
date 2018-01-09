using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Templates.Core;
using Microsoft.Templates.UI.V2Controls;
using Microsoft.Templates.UI.V2Resources;

namespace Microsoft.Templates.UI.V2Extensions
{
    public static class ValidationResultExtensions
    {
        public static Notification GetNotification(this ValidationResult validationResult)
        {
            switch (validationResult.ErrorType)
            {
                case ValidationErrorType.Empty:
                    return Notification.Error(StringRes.NotificationValidationError_Empty, ErrorCategory.NamingValidation, CategoriesToOverride);
                case ValidationErrorType.AlreadyExists:
                    return Notification.Error(StringRes.NotificationValidationError_AlreadyExists, ErrorCategory.NamingValidation, CategoriesToOverride);
                case ValidationErrorType.BadFormat:
                    return Notification.Error(StringRes.NotificationValidationError_BadFormat, ErrorCategory.NamingValidation, CategoriesToOverride);
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
