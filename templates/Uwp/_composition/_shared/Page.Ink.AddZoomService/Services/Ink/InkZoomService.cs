using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Param_ItemNamespace.Services.Ink
{
    public class InkZoomService
    {
        private const float DefaultZoomFactor = 0.1f;
        private readonly ScrollViewer _scrollViewer;

        public InkZoomService(ScrollViewer scrollViewer)
        {
            _scrollViewer = scrollViewer;
        }

        public float ZoomIn(float zoomFactor = DefaultZoomFactor) => ExecuteZoom(_scrollViewer.ZoomFactor + zoomFactor);

        public float ZoomOut(float zoomFactor = DefaultZoomFactor) => ExecuteZoom(_scrollViewer.ZoomFactor - zoomFactor);

        public float ResetZoom() => ExecuteZoom(1f);

        public void FitToScreen()
        {
            if (_scrollViewer.Content is FrameworkElement element)
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

            var ratioWidth = _scrollViewer.ViewportWidth / width;
            var ratioHeight = _scrollViewer.ViewportHeight / height;
            var zoomFactor = (float)Math.Min(ratioWidth, ratioHeight);

            ExecuteZoom(zoomFactor);
        }

        private float ExecuteZoom(float zoomFactor)
        {
            if (_scrollViewer.ChangeView(_scrollViewer.HorizontalOffset, _scrollViewer.VerticalOffset, zoomFactor))
            {
                return zoomFactor;
            }

            return _scrollViewer.ZoomFactor;
        }
    }
}