//{**
//This code block adds the subscription to `App_EnteredBackground` to the App class of your project.
//**}

using System;

//{[{
using Windows.ApplicationModel;
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
//}]}
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
