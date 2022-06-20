//{[{
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Param_RootNamespace.Contracts.Services;
//}]}

namespace Param_RootNamespace.ViewModels;

public class Param_ItemNameDetailViewModel : ObservableRecipient, INavigationAware
{
//{[{
    private readonly INavigationService _navigationService;
//}]}

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
