namespace RootNamespace
{
    sealed partial class App : Application
    {
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    await Services.SuspendAndResumeService.Instance.RestoreStateAsync();
                }
        }
        private async void App_EnteredBackground(object sender, EnteredBackgroundEventArgs e)
        {
            var deferral = e.GetDeferral();
            await Services.SuspendAndResumeService.Instance.SaveStateAsync();
            deferral.Complete();
        }
    }
}