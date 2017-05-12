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
using System.Windows.Media;
using System.Windows.Threading;

using Microsoft.Templates.Core.Mvvm;
using System.Windows.Input;

namespace Microsoft.Templates.UI.ViewModels
{
    public class SummaryItemViewModel : Observable
    {
        public static string SettingsButton = Char.ConvertFromUtf32(0xE713);
        public static string CloseButton = Char.ConvertFromUtf32(0xE013);

        public string Identity { get; set; }
        public string ItemName { get; set; }
        public string TemplateName { get; set; }
        public string Author { get; set; }
        public bool IsRemoveEnabled { get; set; }
        public bool HasDefaultName { get; set; }
        public ICommand OpenCommand { get; set; }
        public ICommand RemoveCommand { get; set; }
        public ICommand SetHomeCommand { get; set; }
        public Action MouseLeaveAction => TryClose;

        private bool _isOpen;
        public bool IsOpen
        {
            get => _isOpen;
            set
            {
                SetProperty(ref _isOpen, value);
                OpenIcon = value ? CloseButton : SettingsButton;
            }
        }

        private string _openIcon = SettingsButton;
        public string OpenIcon
        {
            get => _openIcon;
            private set => SetProperty(ref _openIcon, value);
        }

        private bool _isHome;
        public bool IsHome
        {
            get => _isHome;
            set
            {
                SetProperty(ref _isHome, value);
                ItemFontWeight = value ? FontWeights.Bold : FontWeights.Normal;
            }
        }

        private FontWeight _itemFontWeight = FontWeights.Normal;

        public FontWeight ItemFontWeight
        {
            get => _itemFontWeight;
            set => SetProperty(ref _itemFontWeight, value);
        }



        public string DisplayText => HasDefaultName ? ItemName : $"{ItemName} [{TemplateName}]";

        private Brush _itemForeground = MainViewModel.Current.MainView.FindResource("UIBlue") as SolidColorBrush;
        public Brush ItemForeground
        {
            get => _itemForeground;
            set => SetProperty(ref _itemForeground, value);
        }

        private Brush _authorForeground = MainViewModel.Current.MainView.FindResource("UIBlue") as SolidColorBrush;
        public Brush AuthorForeground
        {
            get => _authorForeground;
            set => SetProperty(ref _authorForeground, value);
        }        

        private DispatcherTimer dt;
        public SummaryItemViewModel()
        {
            dt = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(2)
            };

            dt.Tick += OnTimerTick;

            dt.Start();
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            ItemForeground = MainViewModel.Current.MainView.FindResource("UIBlack") as SolidColorBrush;
            AuthorForeground = MainViewModel.Current.MainView.FindResource("UIGray") as SolidColorBrush;

            dt.Stop();
        }

        internal void TryClose()
        {
            if (_isOpen)
            {
                IsOpen = false;
            }
        }

        internal void TryReleaseHome()
        {
            if (_isHome)
            {
                IsHome = false;
            }
        }
    }
}
