sealed partial class App : Application
{
    //^^
    //{[{
    private ActivationService CreateActivationService()
    {
        return new ActivationService(this, typeof(ViewModels.PivotViewModel));
    }
    //}]}
}