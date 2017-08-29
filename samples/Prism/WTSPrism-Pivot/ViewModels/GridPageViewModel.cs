using System.Collections.ObjectModel;

using Prism.Windows.Mvvm;

using WTSPrism.Models;
using WTSPrism.Services;


namespace WTSPrism.ViewModels
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
