using System;

using DotNetCoreWpfApp.Helpers;

namespace DotNetCoreWpfApp.ViewModels
{
    public class XAMLIslandViewModel : Observable
    {
        private string _text;

        public string Text
        {
            get { return _text; }
            set { Set(ref _text, value); }
        }

        public XAMLIslandViewModel()
        {
        }
    }
}
