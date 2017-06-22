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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

using Microsoft.Templates.UI.ViewModels;

namespace Microsoft.Templates.UI.Controls
{
    public partial class StatusControl : UserControl
    {
        public static StatusViewModel EmptyStatus = new StatusViewModel(StatusType.Empty, string.Empty);

        private DispatcherTimer _hideTimer;

        public StatusViewModel Status
        {
            get => (StatusViewModel)GetValue(StatusProperty);
            set => SetValue(StatusProperty, value);
        }
        public static readonly DependencyProperty StatusProperty = DependencyProperty.Register("Status", typeof(StatusViewModel), typeof(StatusControl), new PropertyMetadata(new StatusViewModel(StatusType.Empty, string.Empty), OnStatusPropertyChanged));

        public string WizardVersion
        {
            get => (string)GetValue(WizardVersionProperty);
            set => SetValue(WizardVersionProperty, value);
        }
        public static readonly DependencyProperty WizardVersionProperty = DependencyProperty.Register("WizardVersion", typeof(string), typeof(StatusControl), new PropertyMetadata(string.Empty, OnVersionInfoChanged));

        public string TemplatesVersion
        {
            get => (string)GetValue(TemplatesVersionProperty);
            set => SetValue(TemplatesVersionProperty, value);
        }
        public static readonly DependencyProperty TemplatesVersionProperty = DependencyProperty.Register("TemplatesVersion", typeof(string), typeof(StatusControl), new PropertyMetadata(string.Empty, OnVersionInfoChanged));

        private static void OnStatusPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as StatusControl;

            control.UpdateStatus(e.NewValue as StatusViewModel);
        }

        private static void OnVersionInfoChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as StatusControl;

            control.UpdateVersionInfo();
        }

        public StatusControl()
        {
            InitializeComponent();

            _hideTimer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(5)
            };

            _hideTimer.Tick += OnHideTick;
        }

        private void OnHideTick(object sender, EventArgs e)
        {
            _hideTimer.Stop();
            Status.Status = StatusType.Empty;
            UpdateStatus(EmptyStatus);
        }

        private void UpdateStatus(StatusViewModel status)
        {
            versionInfoPane.Visibility = Visibility.Collapsed;

            switch (status.Status)
            {
                case StatusType.Information:
                    txtStatus.Text = status.Message;
                    txtIcon.Text = ConvertToChar(SymbolFonts.Completed);
                    txtIcon.Foreground = FindResource("UIBlack") as SolidColorBrush;
                    Background = new SolidColorBrush(Colors.Transparent);
                    break;
                case StatusType.Warning:
                    txtStatus.Text = status.Message;
                    txtIcon.Text = ConvertToChar(SymbolFonts.ErrorBadge);
                    txtIcon.Foreground = FindResource("UIDarkYellow") as SolidColorBrush;
                    // Color yellow = (Color)FindResource("UIYellowColor");
                    // Background = new LinearGradientBrush(yellow, Colors.Transparent, 0);
                    Background = FindResource("UIYellow") as SolidColorBrush;
                    break;
                case StatusType.Error:
                    txtStatus.Text = status.Message;
                    txtIcon.Text = ConvertToChar(SymbolFonts.StatusErrorFull);
                    txtIcon.Foreground = FindResource("UIDarkRed") as SolidColorBrush;
                    // Color red = (Color)FindResource("UIRedColor");
                    // var brush = new LinearGradientBrush(red, Colors.Transparent, 0);
                    var brush = FindResource("UIRed") as SolidColorBrush;
                    brush.Opacity = 0.4;
                    Background = brush;
                    break;
                default:

                    txtStatus.Text = " ";
                    txtIcon.Text = " ";

                    Background = new SolidColorBrush(Colors.Transparent);
                    break;
            }
            UpdateVersionInfo();
            if (status.AutoHide == true)
            {
                _hideTimer.Start();
            }
            else
            {
                _hideTimer.Stop();
            }
        }

        private string ConvertToChar(SymbolFonts font)
        {
            return char.ConvertFromUtf32((int)font);
        }

        private void UpdateVersionInfo() => versionInfoPane.Visibility = HasVersionInfo() && (Status.Status == StatusType.Empty) ? Visibility.Visible : Visibility.Collapsed;

        private bool HasVersionInfo() => !string.IsNullOrEmpty(WizardVersion) && !string.IsNullOrEmpty(TemplatesVersion);
    }
}
