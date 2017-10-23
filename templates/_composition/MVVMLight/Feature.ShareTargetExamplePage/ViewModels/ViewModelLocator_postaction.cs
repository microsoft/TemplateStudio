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
            Register<wts.ItemNameExampleViewModel, wts.ItemNameExamplePage>();
            //}]}
        }

        //{[{
        public wts.ItemNameExampleViewModel wts.ItemNameExampleViewModel => ServiceLocator.Current.GetInstance<wts.ItemNameExampleViewModel>();
        //}]}
    }
}
