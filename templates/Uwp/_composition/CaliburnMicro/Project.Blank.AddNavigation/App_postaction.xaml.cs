namespace Param_RootNamespace
{
    public sealed partial class App
    {
//^^
//{[{

        private ActivationService CreateActivationService()
        {
            return new ActivationService(_container, typeof(ViewModels.Param_HomeNameViewModel));
        }
//}]}
    }
}