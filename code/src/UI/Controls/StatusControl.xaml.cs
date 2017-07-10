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

using Microsoft.Templates.UI.ViewModels.Common;

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

        private static void OnStatusPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as StatusControl;

            control.UpdateStatus(e.NewValue as StatusViewModel);
        }

        public StatusControl()
        {
            InitializeComponent();

            _hideTimer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(7)
            };

            _hideTimer.Tick += OnHideTick;
        }

        private void OnHideTick(object sender, EventArgs e)
        {
            _hideTimer.Stop();
            Hide();
        }

        private void Hide()
        {
            Status.Status = StatusType.Empty;
            UpdateStatus(EmptyStatus);
        }

        private void UpdateStatus(StatusViewModel status)
        {
            switch (status.Status)
            {
                case StatusType.Information:
                    txtStatus.Text = status.Message;
                    txtIcon.Text = ConvertToChar(SymbolFonts.Completed);
                    txtIcon.Foreground = FindResource("UIMiddleDarkBlue") as SolidColorBrush;
                    txtIcon.FontSize = 25;
                    backBackground.Opacity = 1.0;
                    var brush = FindResource("UIBlue") as SolidColorBrush;
                    brush.Opacity = 0.1;
                    shapeBackground.Background = brush;
                    verticalGrid.Background = FindResource("UIMiddleDarkBlue") as SolidColorBrush;
                    iconClose.Foreground = FindResource("UIMiddleDarkGray") as SolidColorBrush;
                    Panel.SetZIndex(this, 2);
                    break;
                case StatusType.Warning:
                    txtStatus.Text = status.Message;
                    txtIcon.Text = ConvertToChar(SymbolFonts.ErrorBadge);
                    txtIcon.Foreground = FindResource("UIDarkYellow") as SolidColorBrush;
                    txtIcon.FontSize = 26;
                    shapeBackground.Background = FindResource("UIYellow") as SolidColorBrush;
                    backBackground.Opacity = 1.0;
                    verticalGrid.Background = FindResource("UIDarkYellow") as SolidColorBrush;
                    iconClose.Foreground = FindResource("UIDarkYellow") as SolidColorBrush;
                    Panel.SetZIndex(this, 2);
                    break;
                case StatusType.Error:
                    txtStatus.Text = status.Message;
                    txtIcon.Text = ConvertToChar(SymbolFonts.StatusErrorFull);
                    txtIcon.Foreground = FindResource("UIDarkRed") as SolidColorBrush;
                    txtIcon.FontSize = 25;
                    brush = FindResource("UIRed") as SolidColorBrush;
                    brush.Opacity = 0.4;
                    backBackground.Opacity = 1.0;
                    verticalGrid.Background = FindResource("UIRed") as SolidColorBrush;
                    iconClose.Foreground = FindResource("UIDarkRed") as SolidColorBrush;
                    shapeBackground.Background = brush;
                    Panel.SetZIndex(this, 2);
                    break;
                default:
                    txtStatus.Text = " ";
                    txtIcon.Text = " ";
                    backBackground.Opacity = 0.0;
                    shapeBackground.Background = new SolidColorBrush(Colors.Transparent);
                    verticalGrid.Background = new SolidColorBrush(Colors.Transparent);
                    iconClose.Foreground = new SolidColorBrush(Colors.Transparent);
                    Panel.SetZIndex(this, 0);
                    break;
            }
            if (status.AutoHideSeconds > 0)
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

        private void OnCloseClicked(object sender, RoutedEventArgs e)
        {
            Hide();
        }
    }
}
