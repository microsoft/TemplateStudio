namespace Param_RootNamespace.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            //^^
            //{[{
            Register<wts.ItemNameDetailViewModel, wts.ItemNameDetailPage>();
            //}]}
        }

        //{[{
        public wts.ItemNameDetailViewModel wts.ItemNameDetailViewModel => ServiceLocator.Current.GetInstance<wts.ItemNameDetailViewModel>();
        //}]}
    }
}
