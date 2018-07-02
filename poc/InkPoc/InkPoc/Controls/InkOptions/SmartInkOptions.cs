using Windows.UI.Xaml.Controls;

namespace InkPoc.Controls
{
    public class SmartInkOptions : InkOption
    {
        private const string TransformTextAndShapesLabel = "Transform text and shapes";

        public AppBarButton TransformTextAndShapesButton { get; set; }

        public SmartInkOptions()
        {
            TransformTextAndShapesButton = InkOptionHelper.BuildAppBarButton(TransformTextAndShapesLabel, "EA80");
        }
    }
}
