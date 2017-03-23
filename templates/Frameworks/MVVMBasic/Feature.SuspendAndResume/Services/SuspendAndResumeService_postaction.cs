private async Task RestoreStateAsync()
{  
    //^^
    if (typeof(Page).IsAssignableFrom(saveState?.Target))
    {
        NavigationService.Navigate(saveState.Target, saveState.SuspensionState);
    }
}
