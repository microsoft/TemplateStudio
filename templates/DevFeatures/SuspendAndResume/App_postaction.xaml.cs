namespace RootNamespace
{
    sealed partial class App : Application
    {
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    await Services.StateService.Instance.RestoreStateAsync(e.PreviousExecutionState, e.Arguments);
                }
        }
        private async void App_EnteredBackground(object sender, EnteredBackgroundEventArgs e)
        {
            var deferral = e.GetDeferral();
            await Services.StateService.Instance.SaveStateAsync();
            deferral.Complete();
        }
    }
}