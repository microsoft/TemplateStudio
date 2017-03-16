sealed partial class App : Application
{
    public App()
    {
        this.InitializeComponent();
        this.EnteredBackground += App_EnteredBackground;
    }
    //^^
    //{[{
        
    private async void App_EnteredBackground(object sender, EnteredBackgroundEventArgs e)
    {
        var deferral = e.GetDeferral();
        await Singleton<Services.SuspendAndResumeService>.Instance.SaveStateAsync();
        deferral.Complete();
    }
    //}]}
}