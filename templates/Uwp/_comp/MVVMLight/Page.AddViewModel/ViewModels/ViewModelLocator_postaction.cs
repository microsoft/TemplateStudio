namespace Param_RootNamespace.ViewModels
{
    public class ViewModelLocator
    {
        private ViewModelLocator()
        {
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
