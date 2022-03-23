namespace Param_RootNamespace.Services
{
    public class ActivationService : IActivationService
    {
        public async Task ActivateAsync(object activationArgs)
        {
            if (App.MainWindow.Content == null)
            {
//{[{
                _shell = Ioc.Default.GetService<ShellPage>();
//}]}
            }
        }
    }
}
