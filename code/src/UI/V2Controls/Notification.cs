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
        Warning,
        Error
    }

    public enum Category
    {
        None,
        TemplatesSync,
        NamingValidation
    }

    public class Notification
    {
        private ICommand _closeCommand;
        private DispatcherTimer _closeTimer;

        public NotificationType Type { get; private set; }

        public Category Category { get; private set; }

        public string Message { get; private set; }

        public ICommand CloseCommand => _closeCommand ?? (_closeCommand = new RelayCommand(OnClose));

        private Notification(bool hasTimer)
        {
            if (hasTimer)
            {
                _closeTimer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(5) };
                _closeTimer.Tick += OnTick;
            }
        }

        public static Notification Information(string message, Category category = Category.None, bool hasTimer = true)
        {
            return new Notification(hasTimer)
            {
                Type = NotificationType.Information,
                Message = message,
                Category = category
            };
        }

        public static Notification Warning(string message, Category category = Category.None, bool hasTimer = true)
        {
            return new Notification(hasTimer)
            {
                Type = NotificationType.Warning,
                Message = message,
                Category = category
            };
        }

        public static Notification Error(string message, Category category = Category.None)
        {
            return new Notification(false)
            {
                Type = NotificationType.Error,
                Message = message,
                Category = category
            };
        }

        private async void OnClose()
        {
            await NotificationsControl.Instance.CloseAsync();
        }

        private async void OnTick(object sender, EventArgs e)
        {
            _closeTimer?.Stop();
            await NotificationsControl.Instance.CloseAsync();
        }

        public void StartCloseTimer()
        {
            _closeTimer?.Start();
        }

        public void StopCloseTimer()
        {
            _closeTimer?.Stop();
        }
    }
}
