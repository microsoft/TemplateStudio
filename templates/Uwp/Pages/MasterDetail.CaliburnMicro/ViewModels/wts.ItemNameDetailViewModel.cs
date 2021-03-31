using System;
using Caliburn.Micro;
using Param_RootNamespace.Core.Models;

namespace Param_RootNamespace.ViewModels
{
    public class wts.ItemNameDetailViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        public wts.ItemNameDetailViewModel(SampleOrder item)
        {
            Item = item;
        }

        public SampleOrder Item { get; }
    }
}
