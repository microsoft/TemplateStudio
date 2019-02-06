using System;
using Caliburn.Micro;
using Param_RootNamespace.Core.Models;

namespace Param_RootNamespace.ViewModels
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
