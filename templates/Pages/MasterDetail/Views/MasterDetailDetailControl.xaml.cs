using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

using Param_ItemNamespace.Models;

namespace Param_ItemNamespace.Views
{
    public sealed partial class MasterDetailDetailControl : UserControl
    {
        public SampleOrder MasterMenuItem
        {
            get { return GetValue(MasterMenuItemProperty) as SampleOrder; }
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

        public static readonly DependencyProperty MasterMenuItemProperty = DependencyProperty.Register("MasterMenuItem", typeof(SampleOrder), typeof(MasterDetailDetailControl), new PropertyMetadata(null));

        public MasterDetailDetailControl()
        {
            InitializeComponent();
        }
    }
}
