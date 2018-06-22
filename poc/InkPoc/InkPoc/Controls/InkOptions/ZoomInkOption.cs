using Windows.UI.Xaml.Controls;

namespace InkPoc.Controls
{
    public class ZoomInkOption : InkOption
    {
        private const string ZoomInLabel = "Zoom in";
        private const string ZoomOutLabel = "Zoom out";

        public AppBarButton ZoomInButton { get; set; }

        public AppBarButton ZoomOutButton { get; set; }

        public ZoomInkOption()
        {
            ZoomInButton = InkOptionHelper.BuildAppBarButton(ZoomInLabel, Symbol.ZoomIn);
            ZoomOutButton = InkOptionHelper.BuildAppBarButton(ZoomOutLabel, Symbol.ZoomOut);
        }
    }
}
