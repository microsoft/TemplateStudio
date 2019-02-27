namespace Param_RootNamespace.Views
{
        public SchemeActivationSamplePage()
        {
            InitializeComponent();
        }
//{[{

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var parameters = e.Parameter as Dictionary<string, string>;
            if (parameters != null)
            {
                ViewModel.Initialize(parameters);
            }
        }
//}]}
}
