using System;
using Param_ItemNamespace.Core.Models;

namespace Param_ItemNamespace.ViewModels
{
    public class ContentGridViewDetailViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        private SampleOrder _item;

        public SampleOrder Item
        {
            get { return _item; }
            set { Set(ref _item, value); }
        }

        public ContentGridViewDetailViewModel()
        {
        }

        public void Initialize(SampleOrder item)
        {
            Item = item;
        }
    }
}
