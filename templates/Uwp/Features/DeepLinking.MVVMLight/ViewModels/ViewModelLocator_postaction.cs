namespace Param_RootNamespace.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            //^^
            //{[{
            Register<SchemeActivationSampleViewModel, SchemeActivationSamplePage>();
            //}]}
        }

        //{[{
        public SchemeActivationSampleViewModel SchemeActivationSampleViewModel => ServiceLocator.Current.GetInstance<SchemeActivationSampleViewModel>();
        //}]}
    }
}
