using Windows.UI.Xaml.Controls;

namespace InkPoc.Controls
{
    public class OpenImageInkOption : InkOption
    {
        private const string OpenImageLabel = "Open image";

        public AppBarButton OpenImageButton { get; set; }

        public OpenImageInkOption()
        {
            OpenImageButton = InkOptionHelper.BuildAppBarButton(OpenImageLabel, "EB9F");
        }
    }
}
