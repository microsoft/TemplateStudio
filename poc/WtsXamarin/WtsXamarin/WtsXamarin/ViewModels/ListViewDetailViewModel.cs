using System;
using System.Collections.Generic;
using System.Text;
using WtsXamarin.Helpers;
using WtsXamarin.Models;

namespace WtsXamarin.ViewModels
{
    public class ListViewDetailViewModel : Observable
    {
        private SampleOrder _item;

        public ListViewDetailViewModel(SampleOrder item)
        {
            Item = item;
        }

        public SampleOrder Item
        {
            get => _item;
            set => Set(ref _item, value);
        }
    }
}
