using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Param_ItemNamespace.Models;


namespace Param_ItemNamespace.Views
{
    public sealed partial class MasterDetailDetailControl : UserControl
    {
        public Order MasterMenuItem
        {
            get { return GetValue(MasterMenuItemProperty) as Order; }
            set
            {
                SetValue(MasterMenuItemProperty, value);
                ConnectedAnimationService.GetForCurrentView().DefaultDuration = System.TimeSpan.FromSeconds(.25);
                ConnectedAnimation iconAnim = ConnectedAnimationService.GetForCurrentView().GetAnimation("companyIcon");
                ConnectedAnimation titleAnim = ConnectedAnimationService.GetForCurrentView().GetAnimation("companyTitle");
                if (!(iconAnim == null) && !(titleAnim == null))
                {
                    iconAnim.TryStart(destIcon);
                    titleAnim.TryStart(destTitle);
                }
            }
        }

        public static DependencyProperty MasterMenuItemProperty = DependencyProperty.Register("MasterMenuItem",typeof(Order),typeof(MasterDetailDetailControl),new PropertyMetadata(null));

        public MasterDetailDetailControl()
        {
            InitializeComponent();
        }
    }
}
