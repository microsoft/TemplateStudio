//{[{
using Param_RootNamespace.Services;
//}]}

namespace Param_RootNamespace
{
    public sealed partial class App : PrismUnityApplication
    {
        public App()
        {
            InitializeComponent();
        }        

        private async Task LaunchApplicationAsync(string page, object launchParam)
        {
            Window.Current.Activate();
//{[{
            Container.RegisterInstance<IConnectedAnimationService>(new ConnectedAnimationService(rootFrame));
//}]}
        }       

    }
}
