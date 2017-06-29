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
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.Extensions;

namespace Microsoft.Templates.UI.Controls
{
    public sealed class OverlayBox : Control
    {
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

        private async void UpdateVisible(bool visible)
        {
            if (visible)
            {
                Panel.SetZIndex(this, 2);
                await this.FadeInAsync();
            }
            else
            {
                Panel.SetZIndex(this, 0);
                await this.FadeOutAsync();
            }
        }
    }
}
