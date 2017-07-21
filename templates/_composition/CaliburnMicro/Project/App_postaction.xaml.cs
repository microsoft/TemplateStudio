 //{[{
using System.Collections.Generic;
using Caliburn.Micro;
//}]}

namespace Param_ItemNamespace
{
    public sealed partial class App
    {
        public App()
        {
            InitializeComponent();
             //^^
            //{[{
            Initialize();
            //}]}
        }

        //^^
        //{[{
        private WinRTContainer _container;

        /// <summary>
        /// Configuration for Caliburn.Micro
        /// </summary>
        protected override void Configure()
        {
            // This configures the framework to map between MainViewModel and MainPage
            // Normally it would map between MainPageViewModel and MainPage
            var config = new TypeMappingConfiguration
            {
                IncludeViewSuffixInViewModelNames = false
            };

            ViewLocator.ConfigureTypeMappings(config);
                
            _container = new WinRTContainer();
            _container.RegisterWinRTServices();

        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }
        //}]}
    }
}