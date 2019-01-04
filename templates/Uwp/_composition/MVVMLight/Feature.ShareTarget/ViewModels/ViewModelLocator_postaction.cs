namespace Param_RootNamespace.ViewModels
{
    public class ViewModelLocator
    {
        private ViewModelLocator()
        {
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
        public wts.ItemNameViewModel wts.ItemNameViewModel => SimpleIoc.Default.GetInstance<wts.ItemNameViewModel>();
        //}]}
    }
}
