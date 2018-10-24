using Param_ItemNamespace.Services.Ink;
using Param_ItemNamespace.Behaviors;
using Param_ItemNamespace.Helpers;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Param_ItemNamespace.Views
{
    // For more information regarding Windows Ink documentation and samples see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/pages/ink.md
    public sealed partial class InkSmartCanvasViewPage : Page
    {
        public InkSmartCanvasViewPage()
        {
            InitializeComponent();

            NavigationViewHeaderBehavior.SetHeaderContext(this, this);
            SetNavigationViewHeader();

            Loaded += (sender, eventArgs) =>
            {
                SetCanvasSize();

                var strokeService = new InkStrokesService(inkCanvas.InkPresenter);
                var analyzer = new InkAsyncAnalyzer(inkCanvas, strokeService);
                var selectionRectangleService = new InkSelectionRectangleService(inkCanvas, selectionCanvas, strokeService);

                ViewModel.Initialize(
                    strokeService,
                    new InkLassoSelectionService(inkCanvas, selectionCanvas, strokeService, selectionRectangleService),
                    new InkNodeSelectionService(inkCanvas, selectionCanvas, analyzer, strokeService, selectionRectangleService),
                    new InkPointerDeviceService(inkCanvas),
                    new InkUndoRedoService(inkCanvas, strokeService),
                    new InkTransformService(drawingCanvas, strokeService),
                    new InkFileService(inkCanvas, strokeService));
            };
        }

        private void SetCanvasSize()
        {
            inkCanvas.Width = Math.Max(canvasScroll.ViewportWidth, 1000);
            inkCanvas.Height = Math.Max(canvasScroll.ViewportHeight, 1000);
        }

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
    }
}
