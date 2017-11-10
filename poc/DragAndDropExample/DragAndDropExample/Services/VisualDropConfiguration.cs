using Windows.UI.Xaml.Media;

namespace DragAndDropExample.Services
{
    public class VisualDropConfiguration
    {
        public string Caption { get; set; } = string.Empty;
        public bool IsCaptionVisible { get; set; } = true;
        public bool IsContentVisible { get; set; } = true;
        public bool IsGlyphVisible { get; set; } = true;
        public ImageSource StartingDragImage { get; set; }
        public ImageSource DropOverImage { get; set; }
    }
}
