// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.Extensions;
using Microsoft.Templates.UI.ViewModels.Common;

namespace Microsoft.Templates.UI.Controls
{
    public sealed class OverlayBox : Control
    {
        private DispatcherTimer _hideTimer;
        private DispatcherTimer HideTimer
        {
            get
            {
                if (_hideTimer == null)
                {
                    _hideTimer = new DispatcherTimer()
                    {
                        Interval = TimeSpan.FromSeconds(5)
                    };
                    _hideTimer.Tick += OnHideTimerTick;
                }
                return _hideTimer;
            }
        }

        public bool Visible
        {
            get => (bool)GetValue(VisibleProperty);
            set => SetValue(VisibleProperty, value);
        }
        public static readonly DependencyProperty VisibleProperty = DependencyProperty.Register("Visible", typeof(bool), typeof(OverlayBox), new PropertyMetadata(true, OnVisiblePropertyChanged));

        public string WizardVersion
        {
            get => (string)GetValue(WizardVersionProperty);
            set => SetValue(WizardVersionProperty, value);
        }
        public static readonly DependencyProperty WizardVersionProperty = DependencyProperty.Register("WizardVersion", typeof(string), typeof(OverlayBox), new PropertyMetadata(string.Empty));

        public string TemplatesVersion
        {
            get => (string)GetValue(TemplatesVersionProperty);
            set => SetValue(TemplatesVersionProperty, value);
        }
        public static readonly DependencyProperty TemplatesVersionProperty = DependencyProperty.Register("TemplatesVersion", typeof(string), typeof(OverlayBox), new PropertyMetadata(string.Empty));

        public bool NewVersionAvailable
        {
            get => (bool)GetValue(NewVersionAvailableProperty);
            set => SetValue(NewVersionAvailableProperty, value);
        }
        public static readonly DependencyProperty NewVersionAvailableProperty = DependencyProperty.Register("NewVersionAvailable", typeof(bool), typeof(OverlayBox), new PropertyMetadata(true));

        public ICommand OpenUrlCommand
        {
            get => (ICommand)GetValue(OpenUrlCommandProperty);
        }
        public static readonly DependencyProperty OpenUrlCommandProperty = DependencyProperty.Register("OpenUrlCommand", typeof(ICommand), typeof(OverlayBox), new PropertyMetadata(new RelayCommand<string>(OpenUrl)));

        public string StatusText
        {
            get => (string)GetValue(StatusTextProperty);
            set => SetValue(StatusTextProperty, value);
        }
        public static readonly DependencyProperty StatusTextProperty = DependencyProperty.Register("StatusText", typeof(string), typeof(OverlayBox), new PropertyMetadata(string.Empty));

        public StatusViewModel Status
        {
            get => (StatusViewModel)GetValue(StatusProperty);
            set => SetValue(StatusProperty, value);
        }
        public static readonly DependencyProperty StatusProperty = DependencyProperty.Register("Status", typeof(StatusViewModel), typeof(OverlayBox), new PropertyMetadata(null, OnStatusPropertyChanged));

        private static void OnStatusPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as OverlayBox;
            if (control != null && control.Status != null)
            {
                control.UpdateStatus(control.Status);
            }
        }

        public ICommand CheckForUpdatesCommand
        {
            get => (ICommand)GetValue(CheckForUpdatesCommandProperty);
            set => SetValue(CheckForUpdatesCommandProperty, value);
        }
        public static readonly DependencyProperty CheckForUpdatesCommandProperty = DependencyProperty.Register("CheckForUpdatesCommand", typeof(ICommand), typeof(OverlayBox), new PropertyMetadata(null));

        public ICommand RefreshCommand
        {
            get => (ICommand)GetValue(RefreshCommandProperty);
            set => SetValue(RefreshCommandProperty, value);
        }
        public static readonly DependencyProperty RefreshCommandProperty = DependencyProperty.Register("RefreshCommand", typeof(ICommand), typeof(OverlayBox), new PropertyMetadata(null));

        private static void OpenUrl(string url)
        {
            if (!string.IsNullOrWhiteSpace(url) && Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                Process.Start(url);
            }
        }

        private static void OnVisiblePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as OverlayBox;
            control.UpdateVisible((bool)e.NewValue);
        }

        static OverlayBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(OverlayBox), new FrameworkPropertyMetadata(typeof(OverlayBox)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (Status != null)
            {
                UpdateStatus(Status);
            }
        }

        private async void UpdateVisible(bool visible)
        {
            if (visible)
            {
                Panel.SetZIndex(this, 2);
                await this.FadeInAsync();
            }
            else
            {
                await this.FadeOutAsync();
                Panel.SetZIndex(this, 0);
            }
        }

        private void UpdateStatus(StatusViewModel status)
        {
            StatusText = status.Message;
            if (status.AutoHide)
            {
                HideTimer.Start();
            }
            else
            {
                HideTimer.Stop();
            }
        }

        private void OnHideTimerTick(object sender, EventArgs e)
        {
            StatusText = string.Empty;
            HideTimer.Stop();
        }
    }
}
