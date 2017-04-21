using Microsoft.Templates.Core.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.UI.ViewModels
{
    public class LicenseViewModel : Observable
    {
        private string _url;
        public string Url
        {
            get => _url;
            set => SetProperty(ref _url, value);
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _templateName;
        public string TemplateName
        {
            get => _templateName;
            set => SetProperty(ref _templateName, value);
        }

    }
}
