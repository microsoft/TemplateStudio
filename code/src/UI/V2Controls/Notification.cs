// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Templates.Core.Mvvm;

namespace Microsoft.Templates.UI.V2Controls
{
    public enum NotificationType
    {
        Information,
        Warning
    }

    public class Notification
    {
        private ICommand _closeCommand;

        public NotificationType Type { get; private set; }

        public string Message { get; private set; }

        public ICommand CloseCommand => _closeCommand ?? (_closeCommand = new RelayCommand(OnClose));

        public static Notification Information(string message)
        {
            return new Notification()
            {
                Type = NotificationType.Information,
                Message = message
            };
        }

        public static Notification Warning(string message)
        {
            return new Notification()
            {
                Type = NotificationType.Warning,
                Message = message
            };
        }

        private async void OnClose()
        {
            await NotificationsControl.Instance.CloseAsync();
        }
    }
}
