private async Task RestoreStateAsync()
{          
    if (typeof(Page).IsAssignableFrom(saveState?.Page))
    {
        //Navigate to page
        var navigationService = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<NavigationService>();
        navigationService.Navigate(navigationService.GetViewModel(saveState.Page), saveState.SuspensionState);
    }
}
