namespace RootNamespace.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            //^^
            Register<uct.ItemNameViewModel, uct.ItemNamePage>();
        }

        public uct.ItemNameViewModel uct.ItemNameViewModel => ServiceLocator.Current.GetInstance<uct.ItemNameViewModel>();
    }
}
