namespace Param_RootNamespace.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            //{[{
            if (SimpleIoc.Default.IsRegistered<NavigationServiceEx>())
            {
                return;
            }
            //}]}
            SimpleIoc.Default.Register(() => new NavigationServiceEx());
            //^^
            //{[{
            Register<wts.ItemNameViewModel, wts.ItemNamePage>();
            //}]}
        }

        //{[{
        public wts.ItemNameViewModel wts.ItemNameViewModel => ServiceLocator.Current.GetInstance<wts.ItemNameViewModel>();
        //}]}
    }
}
