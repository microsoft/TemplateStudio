using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace InkPoc.Services.Ink
{
    public class InkZoomService
    {
        private const float defaultZoomFactor = 0.1f;
        private readonly ScrollViewer scrollViewer;

        public InkZoomService(ScrollViewer _scrollViewer)
        {
            scrollViewer = _scrollViewer;
        }

        public float ZoomIn(float zoomFactor = defaultZoomFactor) => ExecuteZoom(scrollViewer.ZoomFactor + zoomFactor);

        public float ZoomOut(float zoomFactor = defaultZoomFactor) => ExecuteZoom(scrollViewer.ZoomFactor - zoomFactor);

        public float ResetZoom() => ExecuteZoom(1f);

        public void FitToScreen()
        {
            if (scrollViewer.Content is FrameworkElement element)
            {
                FitToSize(element.Width, element.Height);
            }
        }

        public void FitToSize(double width, double height)
        {
            if (width == 0 || height == 0)
            {
                return;
            }

            var ratioWidth = scrollViewer.ViewportWidth / width;
            var ratioHeight = scrollViewer.ViewportHeight / height;
            var zoomFactor = (float)(Math.Min(ratioWidth, ratioHeight));

            ExecuteZoom(zoomFactor);
        }

        private float ExecuteZoom(float zoomFactor)
        {
            if(scrollViewer.ChangeView(scrollViewer.HorizontalOffset, scrollViewer.VerticalOffset, zoomFactor))
            {
                return zoomFactor;
            }

            return scrollViewer.ZoomFactor;
        }
    }
}
