sing System;

namespace Param_RootNamespace.ViewModels
{
    public class ts.ItemNameViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        private string _text;

        public string Text
        {
            get { return _text; }
            set { Param_Setter(ref _text, value); }
        }

        public ts.ItemNameViewModel()
        {
        }
    }
}
