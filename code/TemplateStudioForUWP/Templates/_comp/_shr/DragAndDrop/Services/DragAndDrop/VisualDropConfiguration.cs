using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Param_RootNamespace.Services.DragAndDrop
{
    public class VisualDropConfiguration : DependencyObject
    {
        public static readonly DependencyProperty CaptionProperty =
            DependencyProperty.Register("Caption", typeof(string), typeof(VisualDropConfiguration), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty IsCaptionVisibleProperty =
            DependencyProperty.Register("IsCaptionVisible", typeof(bool), typeof(VisualDropConfiguration), new PropertyMetadata(true));

        public static readonly DependencyProperty IsContentVisibleProperty =
            DependencyProperty.Register("IsContentVisible", typeof(bool), typeof(VisualDropConfiguration), new PropertyMetadata(true));

        public static readonly DependencyProperty IsGlyphVisibleProperty =
            DependencyProperty.Register("IsGlyphVisible", typeof(bool), typeof(VisualDropConfiguration), new PropertyMetadata(true));

        public static readonly DependencyProperty DragStartingImageProperty =
            DependencyProperty.Register("DragStartingImage", typeof(ImageSource), typeof(VisualDropConfiguration), new PropertyMetadata(null));

        public static readonly DependencyProperty DropOverImageProperty =
            DependencyProperty.Register("DropOverImage", typeof(ImageSource), typeof(VisualDropConfiguration), new PropertyMetadata(null));

        public string Caption
        {
            get { return (string)GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }

        public bool IsCaptionVisible
        {
            get { return (bool)GetValue(IsCaptionVisibleProperty); }
            set { SetValue(IsCaptionVisibleProperty, value); }
        }

        public bool IsContentVisible
        {
            get { return (bool)GetValue(IsContentVisibleProperty); }
            set { SetValue(IsContentVisibleProperty, value); }
        }

        public bool IsGlyphVisible
        {
            get { return (bool)GetValue(IsGlyphVisibleProperty); }
            set { SetValue(IsGlyphVisibleProperty, value); }
        }

        public ImageSource DragStartingImage
        {
            get { return (ImageSource)GetValue(DragStartingImageProperty); }
            set { SetValue(DragStartingImageProperty, value); }
        }

        public ImageSource DropOverImage
        {
            get { return (ImageSource)GetValue(DropOverImageProperty); }
            set { SetValue(DropOverImageProperty, value); }
        }
    }
}
