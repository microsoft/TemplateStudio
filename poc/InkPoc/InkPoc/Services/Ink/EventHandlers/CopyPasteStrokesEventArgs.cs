using System.Collections.Generic;
using Windows.UI.Input.Inking;

namespace InkPoc.Services.Ink
{
    public class CopyPasteStrokesEventArgs
    {
        public IEnumerable<InkStroke> Strokes { get; set; }

        public CopyPasteStrokesEventArgs(IEnumerable<InkStroke> strokes)
        {
            Strokes = strokes;
        }
    }
}
