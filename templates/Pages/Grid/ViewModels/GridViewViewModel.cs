using System.Collections.ObjectModel;
using Param_ItemNamespace.Services;

namespace Param_ItemNamespace.ViewModels
{
    public class GridViewViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        public ObservableCollection<SampleDataService.Order> Source
        {
            get
            {
                // TODO UWPTemplates: Replace this with your actual data
                return SampleDataService.GetGridSampleData();
            }
        }
    }
}
