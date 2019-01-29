namespace Param_RootNamespace.ViewModels
{
    public class ViewModelLocator
    {
        private ViewModelLocator()
        {
            //^^
            //{[{
            Register<wts.ItemNameDetailViewModel, wts.ItemNameDetailPage>();
            //}]}
        }

        //{[{
        public wts.ItemNameDetailViewModel wts.ItemNameDetailViewModel => SimpleIoc.Default.GetInstance<wts.ItemNameDetailViewModel>();
        //}]}
    }
}
