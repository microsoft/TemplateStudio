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
using System.Windows.Input;

using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.Resources;

namespace Microsoft.Templates.UI.ViewModels
{
    public class ErrorViewModel : Observable
    {
        private const double HostHeightCollapsed = 250;
        private const double HostHeightExpanded = 500;

        private readonly Window _host;

        public ErrorViewModel(Window host, Exception ex)
        {
            _host = host;

            MessageDetailVisibility = Visibility.Collapsed;

            if (ex == null)
            {
                Message = StringRes.NoData;
            }
            else
            {
                Message = ex.Message;
                MessageDetail = ex.ToString();
            }
        }

        public ICommand CloseCommand => new RelayCommand(() => _host.Close());
        public ICommand ToggleDetailVisibleCommand => new RelayCommand(() => ToggleDetailVisible());

        private double _hostHeight;
        public double HostHeight
        {
            get => _hostHeight;
            set => SetProperty(ref _hostHeight, value);
        }

        private string _toggleButtonIcon;
        public string ToggleButtonIcon
        {
            get => _toggleButtonIcon;
            set => SetProperty(ref _toggleButtonIcon, value);
        }

        private string _message;
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        private string _messageDetail;
        public string MessageDetail
        {
            get => _messageDetail;
            set => SetProperty(ref _messageDetail, value);
        }

        private Visibility _messageDetailVisibility;
        public Visibility MessageDetailVisibility
        {
            get => _messageDetailVisibility;
            set
            {
                SetProperty(ref _messageDetailVisibility, value);

                if (value == Visibility.Visible)
                {
                    HostHeight = HostHeightExpanded;
                    ToggleButtonIcon = "\xE014";
                }
                else
                {
                    HostHeight = HostHeightCollapsed;
                    ToggleButtonIcon = "\xE015";
                }
            }
        }

        private void ToggleDetailVisible()
        {
            if (MessageDetailVisibility == Visibility.Visible)
            {
                MessageDetailVisibility = Visibility.Collapsed;
            }
            else
            {
                MessageDetailVisibility = Visibility.Visible;
            }
        }
    }
}
