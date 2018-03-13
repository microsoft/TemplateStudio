namespace Param_RootNamespace.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
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
