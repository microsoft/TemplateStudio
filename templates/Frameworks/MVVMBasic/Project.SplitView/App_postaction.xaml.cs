sealed partial class App : Application
{
    //^^
    //{[{
    private ActivationService CreateActivationService()
    {
        return new ActivationService(this, typeof(View.MainView), new View.ShellView());
    }
    //}]}
}