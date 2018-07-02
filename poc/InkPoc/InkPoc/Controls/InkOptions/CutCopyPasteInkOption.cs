using Windows.UI.Xaml.Controls;

namespace InkPoc.Controls
{
    public class CutCopyPasteInkOption : InkOption
    {
        private const string CutLabel = "Cut";
        private const string CopyLabel = "Copy";
        private const string PasteLabel = "Paste";

        public AppBarButton CutButton { get; set; }

        public AppBarButton CopyButton { get; set; }

        public AppBarButton PasteButton { get; set; }

        public CutCopyPasteInkOption()
        {
            CutButton = InkOptionHelper.BuildAppBarButton(CutLabel, Symbol.Cut);
            CopyButton = InkOptionHelper.BuildAppBarButton(CopyLabel, Symbol.Copy);
            PasteButton = InkOptionHelper.BuildAppBarButton(PasteLabel, Symbol.Paste);
        }
    }
}
