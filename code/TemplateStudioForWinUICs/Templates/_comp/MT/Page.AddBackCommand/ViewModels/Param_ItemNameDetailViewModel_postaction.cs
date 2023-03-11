//{[{
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Param_RootNamespace.Contracts.Services;
//}]}

namespace Param_RootNamespace.ViewModels;

public partial class Param_ItemNameDetailViewModel : ObservableRecipient, INavigationAware
{
    private readonly ISampleDataService _sampleDataService;
//{[{

    private readonly INavigationService _navigationService;
//}]}
    [ObservableProperty]
    private SampleOrder? item;
//^^
//{[{
    public ICommand GoBackCommand
    {
        get;
    }

//}]}
    public Param_ItemNameDetailViewModel(/*{[{*/INavigationService navigationService/*}]}*/)
    {
//{[{
        _navigationService = navigationService;
//}]}
//^^
//{[{

        GoBackCommand = new RelayCommand(OnGoBack);
//}]}
    }

//^^
//{[{
    private void OnGoBack()
    {
        if (_navigationService.CanGoBack)
        {
            _navigationService.GoBack();
        }
    }
//}]}
}
