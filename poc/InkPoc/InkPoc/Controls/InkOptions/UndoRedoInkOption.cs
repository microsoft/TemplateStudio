using Windows.UI.Xaml.Controls;

namespace InkPoc.Controls
{
    public class UndoRedoInkOption : InkOption
    {
        private const string UndoLabel = "Undo";
        private const string RedoLabel = "Redo";

        public AppBarButton UndoButton { get; set; }

        public AppBarButton RedoButton { get; set; }

        public UndoRedoInkOption()
        {
            UndoButton = InkOptionHelper.BuildAppBarButton(UndoLabel, Symbol.Undo);
            RedoButton = InkOptionHelper.BuildAppBarButton(RedoLabel, Symbol.Redo);
        }
    }
}
