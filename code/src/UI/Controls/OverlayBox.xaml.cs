// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.Extensions;
using Microsoft.Templates.UI.Resources;
using Microsoft.Templates.UI.ViewModels.Common;

namespace Microsoft.Templates.UI.Controls
{
    public partial class OverlayBox : UserControl
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
                        Interval = TimeSpan.FromSeconds(7)
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

        public static readonly DependencyProperty VisibleProperty = DependencyProperty.Register(nameof(Visible), typeof(bool), typeof(OverlayBox), new PropertyMetadata(true, OnVisiblePropertyChanged));

        public string WizardVersion
        {
            get => (string)GetValue(WizardVersionProperty);
            set => SetValue(WizardVersionProperty, value);
        }

        public static readonly DependencyProperty WizardVersionProperty = DependencyProperty.Register(nameof(WizardVersion), typeof(string), typeof(OverlayBox), new PropertyMetadata(string.Empty, OnVersionPropertiesChanged));

        public string TemplatesVersion
        {
            get => (string)GetValue(TemplatesVersionProperty);
            set => SetValue(TemplatesVersionProperty, value);
        }

        public static readonly DependencyProperty TemplatesVersionProperty = DependencyProperty.Register(nameof(TemplatesVersion), typeof(string), typeof(OverlayBox), new PropertyMetadata(string.Empty, OnVersionPropertiesChanged));

        private static void OnVersionPropertiesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as OverlayBox;
            control.UpdateVersionsText();
        }

        public string VersionsText
        {
            get => (string)GetValue(VersionsTextProperty);
            set => SetValue(VersionsTextProperty, value);
        }

        public static readonly DependencyProperty VersionsTextProperty = DependencyProperty.Register(nameof(VersionsText), typeof(string), typeof(OverlayBox), new PropertyMetadata(string.Empty));

        public bool NewVersionAvailable
        {
            get => (bool)GetValue(NewVersionAvailableProperty);
            set => SetValue(NewVersionAvailableProperty, value);
        }

        public static readonly DependencyProperty NewVersionAvailableProperty = DependencyProperty.Register(nameof(NewVersionAvailable), typeof(bool), typeof(OverlayBox), new PropertyMetadata(true));

        public ICommand OpenUrlCommand
        {
            get => (ICommand)GetValue(OpenUrlCommandProperty);
        }

        public static readonly DependencyProperty OpenUrlCommandProperty = DependencyProperty.Register(nameof(OpenUrlCommand), typeof(ICommand), typeof(OverlayBox), new PropertyMetadata(new RelayCommand<string>(OpenUrl)));

        public string StatusText
        {
            get => (string)GetValue(StatusTextProperty);
            set => SetValue(StatusTextProperty, value);
        }

        public static readonly DependencyProperty StatusTextProperty = DependencyProperty.Register(nameof(StatusText), typeof(string), typeof(OverlayBox), new PropertyMetadata(string.Empty));

        public StatusViewModel Status
        {
            get => (StatusViewModel)GetValue(StatusProperty);
            set => SetValue(StatusProperty, value);
        }

        public static readonly DependencyProperty StatusProperty = DependencyProperty.Register(nameof(Status), typeof(StatusViewModel), typeof(OverlayBox), new PropertyMetadata(null, OnStatusPropertyChanged));

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

        public static readonly DependencyProperty CheckForUpdatesCommandProperty = DependencyProperty.Register(nameof(CheckForUpdatesCommand), typeof(ICommand), typeof(OverlayBox), new PropertyMetadata(null));

        public ICommand RefreshCommand
        {
            get => (ICommand)GetValue(RefreshCommandProperty);
            set => SetValue(RefreshCommandProperty, value);
        }

        public static readonly DependencyProperty RefreshCommandProperty = DependencyProperty.Register(nameof(RefreshCommand), typeof(ICommand), typeof(OverlayBox), new PropertyMetadata(null));

        public ICommand RefreshTemplateCacheCommand
        {
            get => (ICommand)GetValue(RefreshTemplateCacheCommandProperty);
            set => SetValue(RefreshTemplateCacheCommandProperty, value);
        }

        public static readonly DependencyProperty RefreshTemplateCacheCommandProperty = DependencyProperty.Register(nameof(RefreshTemplateCacheCommand), typeof(ICommand), typeof(OverlayBox), new PropertyMetadata(null));

        public bool CanForceRefreshTemplateCache
        {
            get => (bool)GetValue(CanForceRefreshTemplateCacheProperty);
            set => SetValue(CanForceRefreshTemplateCacheProperty, value);
        }

        public static readonly DependencyProperty CanForceRefreshTemplateCacheProperty = DependencyProperty.Register(nameof(CanForceRefreshTemplateCache), typeof(bool), typeof(OverlayBox), new PropertyMetadata(false));

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

        public OverlayBox()
        {
            InitializeComponent();
            if (Status != null)
            {
                UpdateStatus(Status);
            }
        }

        private void UpdateVisible(bool visible)
        {
            if (visible)
            {
                Panel.SetZIndex(this, 2);
                this.FadeIn();
            }
            else
            {
                this.FadeOut(0);
                Panel.SetZIndex(this, 0);
            }
        }

        private void UpdateStatus(StatusViewModel status)
        {
            StatusText = status.Message;
            if (status.AutoHideSeconds > 0)
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

        private void UpdateVersionsText()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"{StringRes.WizardVersion} {WizardVersion}");
            sb.AppendLine($"{StringRes.TemplatesVersion} {TemplatesVersion}");
            VersionsText = sb.ToString();
        }
    }
}
