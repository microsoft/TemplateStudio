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

using Microsoft.Templates.UI.Extensions;

namespace Microsoft.Templates.UI.Controls
{
    /// <summary>
    /// Interaction logic for LogoDisplay.xaml
    /// </summary>
    public partial class LogoDisplay : UserControl
    {
        private int animationStatus;
        private DispatcherTimer _isBusyTimer;

        public Brush Fill
        {
            get => (Brush)GetValue(FillProperty);
            set => SetValue(FillProperty, value);
        }
        public static readonly DependencyProperty FillProperty = DependencyProperty.Register("Fill", typeof(Brush), typeof(LogoDisplay), new PropertyMetadata(new SolidColorBrush(Colors.White)));

        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }
        public static readonly DependencyProperty IsBusyProperty = DependencyProperty.Register("IsBusy", typeof(bool), typeof(LogoDisplay), new PropertyMetadata(false, OnIsBusyPropertyChanged));

        public LogoDisplay()
        {
            _isBusyTimer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(250) };
            _isBusyTimer.Tick += OnIsBusyTimerTick;
            InitializeComponent();
        }

        private static void OnIsBusyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (LogoDisplay)d;
            control.UspdateIsBusy((bool)e.NewValue);
        }

        private void UspdateIsBusy(bool isBusy)
        {
            if (isBusy)
            {
                _isBusyTimer.Start();
            }
            else
            {
                _isBusyTimer.Stop();
            }
        }
        private void OnIsBusyTimerTick(object sender, EventArgs e)
        {
            switch (animationStatus)
            {
                case 0:
                    circle1.FadeIn();
                    break;
                case 1:
                    circle2.FadeIn();
                    break;
                case 2:
                    circle3.FadeIn();
                    break;
                case 3:
                    circle1.FadeOut();
                    break;
                case 4:
                    circle2.FadeOut();
                    break;
                case 5:
                    circle3.FadeOut();
                    animationStatus = 0;
                    return;
            }
            animationStatus++;
        }
    }
}
