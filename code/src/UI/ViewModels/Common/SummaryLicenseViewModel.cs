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
using System.Windows.Input;

using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Mvvm;

namespace Microsoft.Templates.UI.ViewModels.Common
{
    public class SummaryLicenseViewModel : Observable
    {
        public SummaryLicenseViewModel()
        {
        }

        public SummaryLicenseViewModel(TemplateLicense license)
        {
            if (license == null)
            {
                return;
            }

            Text = license.Text;
            Url = license.Url;
        }

        private ICommand _navigateCommand;
        public ICommand NavigateCommand => _navigateCommand ?? (_navigateCommand = new RelayCommand(Navigate));

        private void Navigate()
        {
            if (!string.IsNullOrWhiteSpace(Url) && Uri.IsWellFormedUriString(Url, UriKind.Absolute))
            {
                Process.Start(Url);
            }
        }

        private string _text;
        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }

        private string _url;
        public string Url
        {
            get => _url;
            set => SetProperty(ref _url, value);
        }
    }
}
