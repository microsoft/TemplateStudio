sealed partial class App : Application
{
    //^^
    //{[{
    private ActivationService CreateActivationService()
    {
        return new ActivationService(this, typeof(ViewModel.MainViewModel), new View.ShellView());
    }
    //}]}
}