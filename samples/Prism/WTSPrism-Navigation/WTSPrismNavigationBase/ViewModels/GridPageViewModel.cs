using System.Collections.ObjectModel;

using Prism.Windows.Mvvm;

using WTSPrismNavigationBase.Models;
using WTSPrismNavigationBase.Services;


namespace WTSPrismNavigationBase.ViewModels
{
    public class GridPageViewModel : ViewModelBase
    {
        public ObservableCollection<Order> Source
        {
            get
            {
                // TODO WTS: Replace this with your actual data
                return SampleDataService.GetGridSampleData();
            }
        }
    }
}
