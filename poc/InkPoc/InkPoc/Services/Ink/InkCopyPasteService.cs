using System.Linq;
using Windows.Foundation;

namespace InkPoc.Services.Ink
{
    public class InkCopyPasteService
    {
        private Point pastePosition;
        private const int PASTE_DISTANCE = 20;
        private readonly InkStrokesService strokesService;

        public InkCopyPasteService(InkStrokesService _strokesService)
        {
            strokesService = _strokesService;
        }

        public Point Copy()
        {
            if(!CanCopy)
            {
                return new Point();
            }

            var rect = strokesService.CopySelectedStrokes();

            pastePosition = new Point(rect.X, rect.Y);
            return pastePosition;
        }

        public Point Cut()
        {
            if (!CanCut)
            {
                return new Point();
            }

            var rect = strokesService.CutSelectedStrokes();

            pastePosition = new Point(rect.X, rect.Y);
            return pastePosition;
        }
        
        public Rect Paste()
        {
            pastePosition.X += PASTE_DISTANCE;
            pastePosition.Y += PASTE_DISTANCE;

            return Paste(pastePosition);
        }

        public Rect Paste(Point position) => strokesService.PasteSelectedStrokes(position);

        public bool CanCopy => strokesService.GetSelectedStrokes().Any();

        public bool CanCut => strokesService.GetSelectedStrokes().Any();

        public bool CanPaste => strokesService.CanPaste;

    }
}
