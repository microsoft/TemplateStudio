// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

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
        public static StatusViewModel EmptyStatus { get; } = new StatusViewModel(StatusType.Empty, null);

        public StatusType Status { get; set; }

        public string Message { get; set; }

        public bool CanBeCleared { get; set; }

        public int AutoHideSeconds { get; set; }

        public StatusViewModel(StatusType status, string message = null, bool canBeCleared = true, int autoHide = 0)
        {
            Status = status;
            Message = message;
            CanBeCleared = canBeCleared;
            AutoHideSeconds = autoHide;
        }

        public static StatusViewModel Information(string message, bool canBeCleared = true, int autoHide = 0) => new StatusViewModel(StatusType.Information, message, canBeCleared, autoHide);

        public static StatusViewModel Warning(string message, bool canBeCleared = false, int autoHide = 0) => new StatusViewModel(StatusType.Warning, message, canBeCleared, autoHide);

        public static StatusViewModel Error(string message, bool canBeCleared = false, int autoHide = 0) => new StatusViewModel(StatusType.Error, message, canBeCleared, autoHide);
    }
}
