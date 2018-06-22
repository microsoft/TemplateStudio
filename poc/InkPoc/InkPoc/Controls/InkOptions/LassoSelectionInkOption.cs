using Windows.UI.Xaml.Controls;

namespace InkPoc.Controls
{
    public class LassoSelectionInkOption : InkOption
    {
        public const string LassoSelectionButtonTag = "LassoSelection";
        private const string LassoSelectionLabel = "Selection tool";

        public InkToolbarCustomToolButton LassoSelectionButton { get; set; }

        public LassoSelectionInkOption()
        {
            LassoSelectionButton = InkOptionHelper.BuildInkToolbarCustomToolButton(LassoSelectionLabel, "EF20");
        }
    }
}
