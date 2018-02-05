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
        // A Guid is generated as a unique key for each instance as reusing the same VM instance in multiple MediaPlayerElement instances can cause playback errors
        public wts.ItemNameViewModel wts.ItemNameViewModel => ServiceLocator.Current.GetInstance<wts.ItemNameViewModel>(Guid.NewGuid().ToString());
        //}]}
    }
}
