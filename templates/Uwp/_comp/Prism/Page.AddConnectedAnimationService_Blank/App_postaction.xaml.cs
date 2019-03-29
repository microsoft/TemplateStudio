//{[{
using Param_RootNamespace.Services;
using Windows.UI.Xaml.Controls;
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
            //{[{
            var rootFrame = Window.Current.Content as Frame;
            Container.RegisterInstance<IConnectedAnimationService>(new ConnectedAnimationService(rootFrame));
            //}]}
        }

    }
}
