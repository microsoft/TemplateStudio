using Windows.UI.Xaml.Controls;

namespace InkPoc.Controls
{
    public class SaveImageInkOption : InkOption
    {
        private const string SaveImageLabel = "Save image";

        public AppBarButton SaveImageButton { get; set; }

        public SaveImageInkOption()
        {
            SaveImageButton = InkOptionHelper.BuildAppBarButton(SaveImageLabel, "EE71");
        }
    }
}
