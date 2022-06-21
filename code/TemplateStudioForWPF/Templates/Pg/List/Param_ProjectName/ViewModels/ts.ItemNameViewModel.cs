using System.Collections.ObjectModel;
using Param_RootNamespace.Core.Contracts.Services;
using Param_RootNamespace.Core.Models;

namespace Param_RootNamespace.ViewModels;

public class ts.ItemNameViewModel : System.ComponentModel.INotifyPropertyChanged, INavigationAware
{
    private readonly ISampleDataService _sampleDataService;
    private SampleOrder _selected;

    public SampleOrder Selected
    {
        get { return _selected; }
        set { Param_Setter(ref _selected, value); }
    }

    public ObservableCollection<SampleOrder> SampleItems { get; private set; } = new ObservableCollection<SampleOrder>();

    public ts.ItemNameViewModel(ISampleDataService sampleDataService)
    {
        _sampleDataService = sampleDataService;
    }

    public async void OnNavigatedTo(Param_OnNavigatedToParams)
    {
        SampleItems.Clear();

        var data = await _sampleDataService.GetListDetailsDataAsync();

        foreach (var item in data)
        {
            SampleItems.Add(item);
        }

        Selected = SampleItems.First();
    }

    public void OnNavigatedFrom(Param_OnNavigatedFromParams)
    {
    }
}
