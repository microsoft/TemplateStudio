namespace Param_RootNamespace.ViewModels
{
    public class ViewModelLocator
    {
        private ViewModelLocator()
        {
            //^^
            //{[{
            Register<SchemeActivationSampleViewModel, SchemeActivationSamplePage>();
            //}]}
        }

        //{[{
        public SchemeActivationSampleViewModel SchemeActivationSampleViewModel => SimpleIoc.Default.GetInstance<SchemeActivationSampleViewModel>();
        //}]}
    }
}
