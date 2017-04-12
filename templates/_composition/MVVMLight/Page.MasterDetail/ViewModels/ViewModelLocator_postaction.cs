namespace RootNamespace.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            //^^
            Register<uct.ItemNameDetailViewModel, uct.ItemNameDetailPage>();
        }

        public uct.ItemNameDetailViewModel uct.ItemNameDetailViewModel => ServiceLocator.Current.GetInstance<uct.ItemNameDetailViewModel>();
    }
}
