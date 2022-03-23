// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.Windows.Input;
using Microsoft.Templates.Core;
using Microsoft.Templates.UI.Mvvm;

namespace Microsoft.Templates.UI.ViewModels.Common
{
    public class LicenseViewModel : Observable
    {
        private string _text;
        private string _url;
        private ICommand _openCommand;

        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }

        public string Url
        {
            get => _url;
            set => SetProperty(ref _url, value);
        }

        public ICommand OpenCommand => _openCommand ?? (_openCommand = new RelayCommand(OnOpen));

        public LicenseViewModel(TemplateLicense license)
        {
            Text = license.Text;
            Url = license.Url;
        }

        private void OnOpen()
        {
            if (!string.IsNullOrWhiteSpace(Url) && Uri.IsWellFormedUriString(Url, UriKind.Absolute))
            {
                Process.Start(Url);
            }
        }
    }
}
