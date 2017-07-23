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
        public int AutoHideSeconds { get; set; }

        public StatusViewModel(StatusType status, string message = null, int autoHide = 0)
        {
            AutoHideSeconds = autoHide;
            Message = message;
            Status = status;
        }

        public static StatusViewModel Information(string message, int autoHide = 0) => new StatusViewModel(StatusType.Information, message, autoHide);
        public static StatusViewModel Warning(string message, int autoHide = 0) => new StatusViewModel(StatusType.Warning, message, autoHide);
        public static StatusViewModel Error(string message, int autoHide = 0) => new StatusViewModel(StatusType.Error, message, autoHide);
    }
}
