using Windows.UI.Xaml.Controls;

namespace InkPoc.Controls
{
    public class OpenSaveStrokesInkOption : InkOption
    {
        private const string OpenStrokesFileLabel = "Open strokes";
        private const string SaveStrokesFileLabel = "Save strokes";

        public AppBarButton OpenStrokesButton { get; set; }

        public AppBarButton SaveStrokesButton { get; set; }

        public OpenSaveStrokesInkOption()
        {
            OpenStrokesButton = InkOptionHelper.BuildAppBarButton(OpenStrokesFileLabel, "E7C3");
            SaveStrokesButton = InkOptionHelper.BuildAppBarButton(SaveStrokesFileLabel, "E792");
        }
    }
}
