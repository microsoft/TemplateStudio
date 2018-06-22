using Windows.UI.Xaml.Controls;

namespace InkPoc.Controls
{
    public class ClearAllInkOption : InkOption
    {
        private const string ClearAllLabel = "Clear all";

        public AppBarButton ClearAllButton { get; set; }

        public ClearAllInkOption()
        {
            ClearAllButton = InkOptionHelper.BuildAppBarButton(ClearAllLabel, Symbol.Delete);
        }
    }
}
