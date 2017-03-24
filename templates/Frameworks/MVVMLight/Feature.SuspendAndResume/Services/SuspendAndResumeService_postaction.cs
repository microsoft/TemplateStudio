private async Task RestoreStateAsync()
{
    //^^
    var navigationService = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<NavigationServiceEx>();
    navigationService.Navigate(saveState.Target.FullName, saveState.SuspensionState);
}
