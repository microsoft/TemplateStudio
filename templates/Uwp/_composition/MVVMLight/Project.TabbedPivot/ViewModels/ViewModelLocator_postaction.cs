namespace Param_RootNamespace.ViewModels
{
    public class ViewModelLocator
    {
        private ViewModelLocator()
        {
//^^
//{[{
            Register<PivotViewModel, PivotPage>();
//}]}
        }

//{[{
        public PivotViewModel PivotViewModel => ServiceLocator.Current.GetInstance<PivotViewModel>();
//}]}
    }
}
