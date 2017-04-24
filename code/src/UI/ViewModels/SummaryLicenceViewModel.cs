using Microsoft.Templates.Core.Mvvm;

namespace Microsoft.Templates.UI.ViewModels
{
    public class SummaryLicenceViewModel : Observable
    {
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
