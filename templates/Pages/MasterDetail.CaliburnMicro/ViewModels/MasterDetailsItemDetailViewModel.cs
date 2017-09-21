using System;
using Caliburn.Micro;
using Param_ItemNamespace.Models;

namespace Param_ItemNamespace.ViewModels
{
    public class MasterDetailsItemDetailViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        public MasterDetailsItemDetailViewModel(SampleOrder item)
        {
            Item = item;
        }

        public SampleOrder Item { get; }
    }
}
