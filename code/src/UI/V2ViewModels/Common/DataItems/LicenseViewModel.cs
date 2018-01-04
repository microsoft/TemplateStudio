using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Mvvm;

namespace Microsoft.Templates.UI.V2ViewModels.Common
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
