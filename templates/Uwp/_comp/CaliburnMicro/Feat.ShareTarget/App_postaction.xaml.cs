//{[{
using Param_RootNamespace.ViewModels;
//}]}

namespace Param_RootNamespace
{
    public sealed partial class App
    {
        protected override void Configure()
        {
            //^^
            //{[{
            _container.PerRequest<wts.ItemNameViewModel>();
            //}]}
        }
//^^
//{[{

        protected override async void OnShareTargetActivated(ShareTargetActivatedEventArgs args)
        {
            await ActivationService.ActivateFromShareTargetAsync(args);
        }
//}]}
    }
}
