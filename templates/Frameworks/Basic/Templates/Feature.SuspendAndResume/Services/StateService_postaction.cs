namespace ItemNamespace.Services
{
    public class StateService
    {
        public async Task RestoreStateAsync(ApplicationExecutionState previousState, string arguments)
        {
            if (previousState == ApplicationExecutionState.Terminated)
            {
                if (saveState != null && saveState.Page != null)
                {
                    //Navigate to page
                    NavigationService.Navigate(saveState.Page, arguments);
                    //Restore page state
                    RestoreState?.Invoke(this, new RestoreStateEventArgs(saveState.PageState));
                }  
            }
        }
    }
}
