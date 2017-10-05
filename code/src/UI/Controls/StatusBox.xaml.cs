// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.Extensions;
using Microsoft.Templates.UI.ViewModels.Common;

namespace Microsoft.Templates.UI.Controls
{
    /// <summary>
    /// Interaction logic for StatusBox.xaml
    /// </summary>
    public partial class StatusBox : UserControl
    {
        private DispatcherTimer _hideTimer;

        public StatusViewModel Status
        {
            get { return (StatusViewModel)GetValue(StatusProperty); }
            set { SetValue(StatusProperty, value); }
        }
        public static readonly DependencyProperty StatusProperty = DependencyProperty.Register(nameof(Status), typeof(StatusViewModel), typeof(StatusBox), new PropertyMetadata(null, OnStatusPropertyChanged));

        public ICommand CloseCommand
        {
            get { return (ICommand)GetValue(CloseCommandProperty); }
            private set { SetValue(CloseCommandProperty, value); }
        }
        public static readonly DependencyProperty CloseCommandProperty = DependencyProperty.Register(nameof(CloseCommand), typeof(ICommand), typeof(StatusBox), new PropertyMetadata(null));

        private static void OnStatusPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as StatusBox;
            if (control != null)
            {
                control.UpdateStatus(e.NewValue as StatusViewModel);
            }
        }

        public StatusBox()
        {
            _hideTimer = new DispatcherTimer();
            _hideTimer.Tick += OnHideTimerTick;
            CloseCommand = new RelayCommand(OnClose);
            InitializeComponent();
        }

        private void OnClose() => UpdateVisibility(false);

        private void OnHideTimerTick(object sender, EventArgs e)
        {
            HideStatus();
            _hideTimer.Stop();
        }

        private void UpdateStatus(StatusViewModel status)
        {
            var isVisible = status != null && status.Status != StatusType.Empty;
            UpdateVisibility(isVisible, status.AutoHideSeconds);
        }

        private void UpdateVisibility(bool isVisible, int autoHideSeconds = 0)
        {
            if (isVisible)
            {
                Visibility = Visibility.Visible;
                Panel.SetZIndex(this, 2);
                closeButton.Focusable = true;
                if (autoHideSeconds > 0)
                {
                    _hideTimer.Interval = TimeSpan.FromSeconds(autoHideSeconds);
                    _hideTimer.Start();
                }
                else
                {
                    _hideTimer.Stop();
                }
                this.FadeIn();
            }
            else
            {
                HideStatus();
            }
        }

        private void HideStatus()
        {
            this.FadeOut(0);
            Panel.SetZIndex(this, 0);
            closeButton.Focusable = false;
            Visibility = Visibility.Collapsed;
        }
    }
}
