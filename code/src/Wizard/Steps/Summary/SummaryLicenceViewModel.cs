using Microsoft.Templates.Wizard.Dialog;
using Microsoft.Templates.Wizard.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Templates.Wizard.Steps.Summary
{
    public class SummaryLicenceViewModel : ObservableBase
    {
        public ObservableCollection<string> UsedIn { get; } = new ObservableCollection<string>();

        public ICommand ShowLicenceCommand => new RelayCommand(ShowLicence);

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

        private void ShowLicence()
        {
            Process.Start(new ProcessStartInfo(_url));
        }
    }
}
