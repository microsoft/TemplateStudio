// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Templates.UI.ViewModels.Common
{
    public enum StatusType
    {
        Empty,
        Information,
        Warning,
        Error
    }

    public class StatusViewModel
    {
        public static StatusViewModel EmptyStatus = new StatusViewModel(StatusType.Empty);

        public StatusType Status { get; set; }
        public string Message { get; set; }
        public bool CanBeCleaned { get; set; }
        public int AutoHideSeconds { get; set; }

        public StatusViewModel(StatusType status, string message = null, bool canBeCleaned = true, int autoHide = 0)
        {
            Status = status;
            Message = message;
            CanBeCleaned = canBeCleaned;
            AutoHideSeconds = autoHide;
        }

        public static StatusViewModel Information(string message, bool canBeCleaned = true, int autoHide = 0) => new StatusViewModel(StatusType.Information, message, canBeCleaned, autoHide);
        public static StatusViewModel Warning(string message, bool canBeCleaned = false, int autoHide = 0) => new StatusViewModel(StatusType.Warning, message, canBeCleaned, autoHide);
        public static StatusViewModel Error(string message, bool canBeCleaned = false, int autoHide = 0) => new StatusViewModel(StatusType.Error, message, canBeCleaned, autoHide);
    }
}
