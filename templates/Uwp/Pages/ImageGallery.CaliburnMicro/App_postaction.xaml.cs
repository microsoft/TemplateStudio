//{[{
using Param_RootNamespace.ViewModels;
//}]}

namespace Param_ItemNamespace
{
    public sealed partial class App
    {
        protected override void Configure()
        {
            //^^
            //{[{
            _container.PerRequest<ImageGalleryViewDetailViewModel>();
            //}]}
        }

        private UIElement CreateShell()
        {
            var shellPage = new Views.ShellPage();
            //{[{
            _container.RegisterInstance(typeof(IConnectedAnimationService), nameof(IConnectedAnimationService), new ConnectedAnimationService(shellPage.GetFrame()));
            //}]}
            return shellPage;
        }
    }
}
