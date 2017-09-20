using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Composition;
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
                block.Visibility = Visibility.Collapsed;
                ConnectedAnimation iconAnim = ConnectedAnimationService.GetForCurrentView().GetAnimation("companyIcon");
                ConnectedAnimation titleAnim = ConnectedAnimationService.GetForCurrentView().GetAnimation("companyTitle");
                var a = value;
                if (!(iconAnim == null) && !(titleAnim == null))
                {
                    
                    iconAnim.Completed += (sender_, e_) =>
                    {
                        destPanel.Opacity = 1;
                        block.Visibility = Visibility.Visible;

                    };
                    iconAnim.TryStart(destIcon);
                    titleAnim.TryStart(destTitle);
                    destPanel.Opacity = .005;
                    
                }
                else
                {
                    destPanel.Opacity = 1;
                    block.Visibility = Visibility.Visible;
                }
                SetValue(MasterMenuItemProperty, value);
            }
        }

        public static DependencyProperty MasterMenuItemProperty = DependencyProperty.Register("MasterMenuItem",typeof(SampleOrder),typeof(MasterDetailDetailControl),new PropertyMetadata(null));
        private Compositor _compositor;

        public MasterDetailDetailControl()
        {
            InitializeComponent();
            _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;

            // Add a translation animation that will play when this element is shown
            var topBorderOffsetAnimation = _compositor.CreateScalarKeyFrameAnimation();
            topBorderOffsetAnimation.Duration = TimeSpan.FromMilliseconds(200);
            topBorderOffsetAnimation.Target = "Translation.Y";
            topBorderOffsetAnimation.InsertKeyFrame(0, 1000.0f);
            topBorderOffsetAnimation.InsertKeyFrame(1, 0);

            ElementCompositionPreview.SetIsTranslationEnabled(block, true);
            // Call GetElementVisual() to work around a bug in Insider Build 15025
            ElementCompositionPreview.GetElementVisual(block);
            ElementCompositionPreview.SetImplicitShowAnimation(block, topBorderOffsetAnimation);
        }
    }
}
