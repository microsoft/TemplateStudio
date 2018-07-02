using Windows.UI.Xaml.Controls;

namespace InkPoc.Controls
{
    public class TouchInkingInkOption : InkOption
    {
        private const string TouchInkingLabel = "Touch inking";

        public InkToolbarCustomToggleButton TouchInkingButton { get; set; }

        public TouchInkingInkOption()
        {
            TouchInkingButton = InkOptionHelper.BuildInkToolbarCustomToggleButton(TouchInkingLabel, "ED5F");
        }
    }
}
