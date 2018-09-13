namespace Param_ItemNamespace.Views
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
                Initialize(parameters);
            }
        }
//}]}
}
