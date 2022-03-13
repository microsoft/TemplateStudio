//{**
//This code block adds the subscription to `App_EnteredBackground` to the App class of your project.
//**}

using System;

//{[{
using Windows.ApplicationModel;
using Param_RootNamespace.Core.Helpers;
//}]}

namespace Param_RootNamespace
{
    public sealed partial class App : Application
    {
        public App()
        {
            InitializeComponent();

//{[{
            EnteredBackground += App_EnteredBackground;
            Resuming += App_Resuming;
//}]}
        }
//^^
//{[{

        private async void App_EnteredBackground(object sender, EnteredBackgroundEventArgs e)
        {
            var deferral = e.GetDeferral();
            await Singleton<SuspendAndResumeService>.Instance.SaveStateAsync();
            deferral.Complete();
        }

        private void App_Resuming(object sender, object e)
        {
            Singleton<SuspendAndResumeService>.Instance.ResumeApp();
        }
//}]}
    }
}
