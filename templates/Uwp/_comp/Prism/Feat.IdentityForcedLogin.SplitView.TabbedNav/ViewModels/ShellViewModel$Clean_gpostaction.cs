//{--{
public ShellViewModel(INavigationService navigationServiceInstance)
{
    _navigationService = navigationServiceInstance;
    ItemInvokedCommand = new DelegateCommand<WinUI.NavigationViewItemInvokedEventArgs>(OnItemInvoked);
}
//}--}