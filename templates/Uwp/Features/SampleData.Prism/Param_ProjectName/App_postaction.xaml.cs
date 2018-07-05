using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Param_RootNamespace.Services;
//{[{
using Param_RootNamespace.Core.Services;
//}]}
        public App()
        {
            InitializeComponent();
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
//^^
//{[{
            Container.RegisterType<ISampleDataService, SampleDataService>();
//}]}
        }
    }
}
