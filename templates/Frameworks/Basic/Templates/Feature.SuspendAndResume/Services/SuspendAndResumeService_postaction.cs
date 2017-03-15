namespace ItemNamespace.Services
{
    public class SuspendAndResumeService
    {
        public async Task RestoreStateAsync()
        {           
            if (typeof(Page).IsAssignableFrom(saveState?.Page))
            {
                //Navigate to page
                NavigationService.Navigate(saveState.Page, saveState.SuspensionState);
            }
        }
    }
}
