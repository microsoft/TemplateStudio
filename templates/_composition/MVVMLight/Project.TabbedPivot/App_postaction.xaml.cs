namespace Param_RootNamespace
{
    public sealed partial class App
	{
//^^
//{[{

        private ActivationService CreateActivationService()
        {
            return new ActivationService(this, typeof(ViewModels.PivotViewModel));
        }
//}]}
	}
}
