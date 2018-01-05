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
                    return Notification.Error(StringRes.NotificationValidationError_Empty, Category.NamingValidation);
                case ValidationErrorType.AlreadyExists:
                    return Notification.Error(StringRes.NotificationValidationError_AlreadyExists, Category.NamingValidation);
                case ValidationErrorType.BadFormat:
                    return Notification.Error(StringRes.NotificationValidationError_BadFormat, Category.NamingValidation);
                case ValidationErrorType.ReservedName:
                    return Notification.Error(StringRes.NotificationValidationError_ReservedName, Category.NamingValidation);
                default:
                    return null;
            }
        }
    }
}
