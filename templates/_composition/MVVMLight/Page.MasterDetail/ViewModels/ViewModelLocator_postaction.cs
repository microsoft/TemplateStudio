namespace RootNamespace.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            //^^
            Registeruct.ItemNameDetail(navigationService);
        }

        public uct.ItemNameDetailViewModel uct.ItemNameDetailViewModel => ServiceLocator.Current.GetInstance<uct.ItemNameDetailViewModel>();
        //{[{
        public void Registeruct.ItemNameDetail(NavigationServiceEx navigationService)
        {
            SimpleIoc.Default.Register<uct.ItemNameDetailViewModel>();
            navigationService.Configure(typeof(uct.ItemNameDetailViewModel).FullName, typeof(uct.ItemNameDetailPage));
        }
        //}]}
    }
}
