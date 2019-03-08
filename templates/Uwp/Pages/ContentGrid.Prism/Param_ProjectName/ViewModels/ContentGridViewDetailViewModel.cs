using System;
using System.Collections.Generic;
using System.Linq;
using Param_RootNamespace.Core.Models;
using Param_RootNamespace.Core.Services;
using Param_RootNamespace.Services;

using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using Windows.UI.Xaml.Navigation;

namespace Param_RootNamespace.ViewModels
{
    public class ContentGridViewDetailViewModel : ViewModelBase
    {
        private readonly ISampleDataService _sampleDataService;
        private readonly IConnectedAnimationService _connectedAnimationService;

        private SampleOrder _item;

        public SampleOrder Item
        {
            get { return _item; }
            set { SetProperty(ref _item, value); }
        }

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            if (e.Parameter is long orderId)
            {
                Item = _sampleDataService.GetContentGridData().First(i => i.OrderId == orderId);
            }
        }

        public override void OnNavigatingFrom(NavigatingFromEventArgs e, Dictionary<string, object> viewModelState, bool suspending)
        {
            base.OnNavigatingFrom(e, viewModelState, suspending);
            if (e.NavigationMode == NavigationMode.Back)
            {
                _connectedAnimationService.SetListDataItemForNextConnectedAnimation(Item);
            }
        }
    }
}
