using System;
using Param_ItemNamespace.Core.Models;
using Prism.Windows.Mvvm;

namespace Param_ItemNamespace.ViewModels
{
    public class ContentGridViewDetailViewModel : ViewModelBase
    {
        private SampleOrder _item;

        public SampleOrder Item
        {
            get { return _item; }
            set { SetProperty(ref _item, value); }
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
