Imports Windows.Foundation

Namespace Services.Ink
    Public Class InkCopyPasteService
        Private Const PasteDistance As Integer = 20
        Private ReadOnly _strokesService As InkStrokesService
        Private pastePosition As Point

        Public Sub New(strokesService As InkStrokesService)
            _strokesService = strokesService
        End Sub

        Public Function Copy() As Point
            If Not CanCopy Then
                Return Nothing
            End If

            Dim rect = _strokesService.CopySelectedStrokes()
            pastePosition = New Point(rect.X, rect.Y)
            Return pastePosition
        End Function

        Public Function Cut() As Point
            If Not CanCut Then
                Return Nothing
            End If

            Dim rect = _strokesService.CutSelectedStrokes()
            pastePosition = New Point(rect.X, rect.Y)
            Return pastePosition
        End Function

        Public Function Paste() As Rect
            pastePosition.X += PasteDistance
            pastePosition.Y += PasteDistance
            Return Paste(pastePosition)
        End Function

        Public Function Paste(position As Point) As Rect
            Return _strokesService.PasteSelectedStrokes(position)
        End Function

        Public ReadOnly Property CanCopy As Boolean
            Get
                Return _strokesService.GetSelectedStrokes().Any()
            End Get
        End Property

        Public ReadOnly Property CanCut As Boolean
            Get
                Return _strokesService.GetSelectedStrokes().Any()
            End Get
        End Property

        Public ReadOnly Property CanPaste As Boolean
            Get
                Return _strokesService.CanPaste
            End Get
        End Property
    End Class
End Namespace
