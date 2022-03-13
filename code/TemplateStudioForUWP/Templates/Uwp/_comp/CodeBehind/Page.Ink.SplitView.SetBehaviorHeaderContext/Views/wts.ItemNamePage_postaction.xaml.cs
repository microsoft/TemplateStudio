//{[{
using Param_RootNamespace.Behaviors;
using Windows.UI.Xaml.Data;
//}]}

namespace Param_RootNamespace.Views
{
    public sealed partial class wts.ItemNamePage : Page, INotifyPropertyChanged
    {
        public wts.ItemNamePage()
        {
            InitializeComponent();
            //{[{
            SetNavigationViewHeaderContext();
            SetNavigationViewHeaderTemplate();
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

        private void VisualStateGroup_CurrentStateChanged(object sender, VisualStateChangedEventArgs e) => SetNavigationViewHeaderTemplate();

        private void SetNavigationViewHeaderTemplate()
        {
            if (visualStateGroup.CurrentState != null)
            {
                switch (visualStateGroup.CurrentState.Name)
                {
                    case "BigVisualState":
                        NavigationViewHeaderBehavior.SetHeaderTemplate(this, Resources["BigHeaderTemplate"] as DataTemplate);
                        bottomCommandBar.Visibility = Visibility.Collapsed;
                        break;
                    case "SmallVisualState":
                        NavigationViewHeaderBehavior.SetHeaderTemplate(this, Resources["SmallHeaderTemplate"] as DataTemplate);
                        bottomCommandBar.Visibility = Visibility.Visible;
                        break;
                }
            }
        }

        private void SetNavigationViewHeaderContext()
        {
            var headerContextBinding = new Binding
            {
                Source = this,
                Mode = BindingMode.OneWay,
            };

            SetBinding(NavigationViewHeaderBehavior.HeaderContextProperty, headerContextBinding);
        }
        //}]}
    }
}
