//{[{
using Prism.Mvvm;
using System.Globalization;
//}]}namespace Param_RootNamespace
{
    public sealed partial class App : PrismUnityApplication
    {
//^^
//{[{
        protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
            NavigationService.Navigate(PageTokens.Param_HomeNamePage, null);
            return Task.FromResult(true);
        }
//}]}

        protected override Task OnInitializeAsync(IActivatedEventArgs args)
        {
            Container.RegisterInstance<IResourceLoader>(new ResourceLoaderAdapter(new ResourceLoader()));
//{[{
            // We are remapping the default ViewNamePage->ViewNamePageViewModel naming to ViewNamePage->ViewNameViewModel to 
            // gain better code reuse with other frameworks and pages within Windows Template Studio
            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
            {
                var viewModelTypeName = string.Format(CultureInfo.InvariantCulture, "Param_RootNamespace.ViewModels.{0}ViewModel, Param_RootNamespace", viewType.Name.Replace("Page",String.Empty));
                var viewModelType = Type.GetType(viewModelTypeName);
                return viewModelType;
            });
//}]}
            return base.OnInitializeAsync(args);
        }

    }
}
