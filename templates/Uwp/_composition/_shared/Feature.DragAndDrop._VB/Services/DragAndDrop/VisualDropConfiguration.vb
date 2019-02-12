Namespace Services.DragAndDrop

    Public Class VisualDropConfiguration
        Inherits DependencyObject

        Public Shared ReadOnly CaptionProperty As DependencyProperty = DependencyProperty.Register("Caption", GetType(String), GetType(VisualDropConfiguration), New PropertyMetadata(String.Empty))

        Public Shared ReadOnly IsCaptionVisibleProperty As DependencyProperty = DependencyProperty.Register("IsCaptionVisible", GetType(Boolean), GetType(VisualDropConfiguration), New PropertyMetadata(True))

        Public Shared ReadOnly IsContentVisibleProperty As DependencyProperty = DependencyProperty.Register("IsContentVisible", GetType(Boolean), GetType(VisualDropConfiguration), New PropertyMetadata(True))

        Public Shared ReadOnly IsGlyphVisibleProperty As DependencyProperty = DependencyProperty.Register("IsGlyphVisible", GetType(Boolean), GetType(VisualDropConfiguration), New PropertyMetadata(True))

        Public Shared ReadOnly DragStartingImageProperty As DependencyProperty = DependencyProperty.Register("DragStartingImage", GetType(ImageSource), GetType(VisualDropConfiguration), New PropertyMetadata(Nothing))

        Public Shared ReadOnly DropOverImageProperty As DependencyProperty = DependencyProperty.Register("DropOverImage", GetType(ImageSource), GetType(VisualDropConfiguration), New PropertyMetadata(Nothing))

        Public Property Caption As String
            Get
                Return CStr(GetValue(CaptionProperty))
            End Get

            Set(value As String)
                SetValue(CaptionProperty, value)
            End Set
        End Property

        Public Property IsCaptionVisible As Boolean
            Get
                Return CBool(GetValue(IsCaptionVisibleProperty))
            End Get

            Set(value As Boolean)
                SetValue(IsCaptionVisibleProperty, value)
            End Set
        End Property

        Public Property IsContentVisible As Boolean
            Get
                Return CBool(GetValue(IsContentVisibleProperty))
            End Get

            Set(value As Boolean)
                SetValue(IsContentVisibleProperty, value)
            End Set
        End Property

        Public Property IsGlyphVisible As Boolean
            Get
                Return CBool(GetValue(IsGlyphVisibleProperty))
            End Get

            Set(value As Boolean)
                SetValue(IsGlyphVisibleProperty, value)
            End Set
        End Property

        Public Property DragStartingImage As ImageSource
            Get
                Return CType(GetValue(DragStartingImageProperty), ImageSource)
            End Get

            Set(value As ImageSource)
                SetValue(DragStartingImageProperty, value)
            End Set
        End Property

        Public Property DropOverImage As ImageSource
            Get
                Return CType(GetValue(DropOverImageProperty), ImageSource)
            End Get

            Set(value As ImageSource)
                SetValue(DropOverImageProperty, value)
            End Set
        End Property
    End Class
End Namespace
