using System;
using System.Collections.ObjectModel;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Param_RootNamespace.Core.Models;
using Param_RootNamespace.Core.Services;
using Param_RootNamespace.Services;

namespace Param_RootNamespace.Views
{
    public sealed partial class ContentGridViewPage : Page, System.ComponentModel.INotifyPropertyChanged
    {
        private ObservableCollection<SampleOrder> _source;

        public ObservableCollection<SampleOrder> Source
        {
            get
            {
                return _source;
            }

            set
            {
                Set(ref _source, value);
            }
        }

        public ContentGridViewPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // TODO WTS: Replace this with your actual data
            Source = await SampleDataService.GetContentGridDataAsync();
        }

        private void OnItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is SampleOrder item)
            {
                NavigationService.Frame.SetListDataItemForNextConnectedAnimation(item);
                NavigationService.Navigate<ContentGridViewDetailPage>(item.OrderId);
            }
        }
    }
}
