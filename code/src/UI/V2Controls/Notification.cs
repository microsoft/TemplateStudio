// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.Templates.Core.Mvvm;

namespace Microsoft.Templates.UI.V2Controls
{
    public enum NotificationType
    {
        Information,
        Warning
    }

    public enum ReplacementCategory
    {
        None,
        TemplatesSync
    }

    public class Notification
    {
        private ICommand _closeCommand;
        private DispatcherTimer _closeTimer;

        public NotificationType Type { get; private set; }

        public ReplacementCategory ReplacementCategory { get; private set; }

        public string Message { get; private set; }

        public ICommand CloseCommand => _closeCommand ?? (_closeCommand = new RelayCommand(OnClose));

        private Notification()
        {
            _closeTimer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(5) };
            _closeTimer.Tick += OnTick;
        }

        public static Notification Information(string message, ReplacementCategory replacementCategory = ReplacementCategory.None)
        {
            return new Notification()
            {
                Type = NotificationType.Information,
                Message = message,
                ReplacementCategory = replacementCategory
            };
        }

        public static Notification Warning(string message, ReplacementCategory replacementCategory = ReplacementCategory.None)
        {
            return new Notification()
            {
                Type = NotificationType.Warning,
                Message = message,
                ReplacementCategory = replacementCategory
            };
        }

        private async void OnClose()
        {
            await NotificationsControl.Instance.CloseAsync();
        }

        private async void OnTick(object sender, EventArgs e)
        {
            _closeTimer.Stop();
            await NotificationsControl.Instance.CloseAsync();
        }

        public void StartCloseTimer()
        {
            _closeTimer.Start();
        }

        public void StopCloseTimer()
        {
            _closeTimer.Stop();
        }
    }
}
