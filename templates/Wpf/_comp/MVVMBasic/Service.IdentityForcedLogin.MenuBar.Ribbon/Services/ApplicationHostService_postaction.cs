namespace Param_RootNamespace.Services
{
    public class ApplicationHostService : IHostedService
    {
        private void OnLoggedIn(object sender, EventArgs e)
        {
            _navigationService.Initialize(_shellWindow.GetNavigationFrame());
//{[{
            _rightPaneService.Initialize(_shellWindow.GetRightPaneFrame(), _shellWindow.GetSplitView());
//}]}
            _shellWindow.ShowWindow();
        }
    }
}
