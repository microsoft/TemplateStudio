using System;
using Windows.ApplicationModel;

namespace RootNamespace
{
    sealed partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            EnteredBackground += App_EnteredBackground;
        }
        //^^
        //{[{
            
        private async void App_EnteredBackground(object sender, EnteredBackgroundEventArgs e)
        {
            var deferral = e.GetDeferral();
            await Helpers.Singleton<SuspendAndResumeService>.Instance.SaveStateAsync();
            deferral.Complete();
        }
        //}]}
    }
}