namespace Param_RootNamespace.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
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
