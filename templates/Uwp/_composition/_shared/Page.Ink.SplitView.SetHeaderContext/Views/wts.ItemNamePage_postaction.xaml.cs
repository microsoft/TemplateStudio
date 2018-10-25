namespace Param_ItemNamespace.Views
{
    public sealed partial class wts.ItemNamePage : Page
    {
        public wts.ItemNamePage()
        {
            InitializeComponent();
            //{[{;
            SetNavigationViewHeader();
            //}]}
        }

        //{[{
        private void OnInkToolbarLoaded(object sender, RoutedEventArgs e)
        {
            if (sender is InkToolbar inkToolbar)
            {
                inkToolbar.TargetInkCanvas = inkCanvas;
            }
        }

        private void VisualStateGroup_CurrentStateChanged(object sender, VisualStateChangedEventArgs e) => SetNavigationViewHeader();

        private void SetNavigationViewHeader()
        {
            if (visualStateGroup.CurrentState != null)
            {
                switch (visualStateGroup.CurrentState.Name)
                {
                    case "BigVisualState":
                        headerContent.ContentTemplate = Resources["BigHeaderTemplate"] as DataTemplate;
                        bottomCommandBar.Visibility = Visibility.Collapsed;
                        break;
                    case "SmallVisualState":
                        headerContent.ContentTemplate = Resources["SmallHeaderTemplate"] as DataTemplate;
                        bottomCommandBar.Visibility = Visibility.Visible;
                        break;
                }
            }
        }

        //}]}
    }
}
