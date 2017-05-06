sealed partial class App : Application
{
    //^^
    //{[{
        
    private ActivationService CreateActivationService()
    {
        return new ActivationService(this, typeof(Views.MainPage), new Views.ShellPage()); //REP:LAUNCH|PRI:1
    }
    //}]}
}