namespace ItemNamespace.Services
{
    public class StateService
    {
        private void NavigateToPage(Type page, string arguments)
        {
            NavigationService.Navigate(page, arguments);
        }
    }
}
