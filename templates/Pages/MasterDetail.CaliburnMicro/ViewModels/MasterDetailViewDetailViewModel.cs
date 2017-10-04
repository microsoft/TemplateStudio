using System;
using Caliburn.Micro;
using Param_ItemNamespace.Models;

namespace Param_ItemNamespace.ViewModels
{
    public class MasterDetailViewDetailViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        public MasterDetailViewDetailViewModel(SampleOrder item)
        {
            Item = item;
        }

        public SampleOrder Item { get; }
    }
}
