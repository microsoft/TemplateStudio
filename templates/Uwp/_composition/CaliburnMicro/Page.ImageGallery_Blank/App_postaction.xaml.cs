//{[{
using Windows.UI.Xaml.Controls;
//}]}
namespace Param_ItemNamespace
{
    public sealed partial class App
    {
        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            if (!args.PrelaunchActivated)
            {
//^^
//{[{
                var frame = Window.Current.Content as Frame;
                _container.RegisterInstance(typeof(IConnectedAnimationService), nameof(IConnectedAnimationService), new ConnectedAnimationService(frame));
//}]}
            }
        }
    }
}
