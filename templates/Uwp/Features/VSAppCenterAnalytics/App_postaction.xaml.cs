
using System;

//{[{
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
//}]}

namespace Param_RootNamespace
{
    public sealed partial class App : Application
    {
        public App()
        {
            InitializeComponent();

//{[{
            // TODO WTS: Add your app in the app center and set your secret here. More at https://docs.microsoft.com/en-us/appcenter/sdk/getting-started/uwp
            AppCenter.Start("{Your App Secret}", typeof(Analytics));
//}]}
        }
    }
}
