namespace Param_RootNamespace
{
    public sealed partial class App
    {
        protected override void Configure()
        {
            //^^
            //{[{       
            _container.PerRequest<PivotViewModel>();
            //}]}
        }
//^^
//{[{

        private ActivationService CreateActivationService()
        {
            return new ActivationService(_container, typeof(PivotViewModel));
        }
//}]}
    }
}
