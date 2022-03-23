Imports System.Numerics
Imports Windows.Storage
Imports Windows.Storage.Provider
Imports Windows.UI.Input.Inking
Imports Windows.UI.Input.Inking.Analysis
Imports Param_RootNamespace.EventHandlers.Ink

Namespace Services.Ink
    Public Class InkStrokesService
        Private ReadOnly _strokeContainer As InkStrokeContainer

        Public Sub New(inkPresenter As InkPresenter)
            _strokeContainer = inkPresenter.StrokeContainer
            AddHandler inkPresenter.StrokesCollected, Sub(s, e) RaiseEvent StrokesCollected(Me, e)
            AddHandler inkPresenter.StrokesErased, Sub(s, e) RaiseEvent StrokesErased(Me, e)
        End Sub

        Public Event StrokesCollected As EventHandler(Of InkStrokesCollectedEventArgs)

        Public Event StrokesErased As EventHandler(Of InkStrokesErasedEventArgs)

        Public Event AddStrokeEvent As EventHandler(Of AddStrokeEventArgs)

        Public Event RemoveStrokeEvent As EventHandler(Of RemoveEventArgs)

        Public Event MoveStrokesEvent As EventHandler(Of MoveStrokesEventArgs)

        Public Event ClearStrokesEvent As EventHandler(Of EventArgs)

        Public Event CopyStrokesEvent As EventHandler(Of CopyPasteStrokesEventArgs)

        Public Event CutStrokesEvent As EventHandler(Of CopyPasteStrokesEventArgs)

        Public Event PasteStrokesEvent As EventHandler(Of CopyPasteStrokesEventArgs)

        Public Event LoadInkFileEvent As EventHandler(Of EventArgs)

        Public Event SelectStrokesEvent As EventHandler(Of EventArgs)

        Public Function AddStroke(stroke As InkStroke) As InkStroke
            Dim newStroke = stroke.Clone()
            _strokeContainer.AddStroke(newStroke)
            RaiseEvent AddStrokeEvent(Me, New AddStrokeEventArgs(newStroke, stroke))
            Return newStroke
        End Function

        Public Function RemoveStroke(stroke As InkStroke) As Boolean
            Dim deleteStroke = GetStrokes().FirstOrDefault(Function(s) s.Id = stroke.Id)

            If deleteStroke Is Nothing Then
                Return False
            End If

            ClearStrokesSelection()
            deleteStroke.Selected = True
            _strokeContainer.DeleteSelected()
            RaiseEvent RemoveStrokeEvent(Me, New RemoveEventArgs(stroke))
            Return True
        End Function

        Public Function RemoveStrokesByIds(strokeIds As IEnumerable(Of UInteger)) As Boolean
            Dim strokes = GetStrokesByIds(strokeIds)

            For Each stroke In strokes
                RemoveStroke(stroke)
            Next

            Return strokes.Any()
        End Function

        Public Function GetStrokes() As IEnumerable(Of InkStroke)
            Return _strokeContainer.GetStrokes()
        End Function

        Public Function GetSelectedStrokes() As IEnumerable(Of InkStroke)
            Return GetStrokes().Where(Function(stroke) stroke.Selected)
        End Function

        Public Sub ClearStrokes()
            _strokeContainer.Clear()
            RaiseEvent ClearStrokesEvent(Me, EventArgs.Empty)
        End Sub

        Public Sub ClearStrokesSelection()
            For Each stroke In GetStrokes()
                stroke.Selected = False
            Next
            RaiseEvent SelectStrokesEvent(Me, EventArgs.Empty)
        End Sub

        Public Function SelectStrokes(strokes As IEnumerable(Of InkStroke)) As Rect
            ClearStrokesSelection()

            For Each stroke In strokes
                stroke.Selected = True
            Next

            RaiseEvent SelectStrokesEvent(Me, EventArgs.Empty)
            Return GetRectBySelectedStrokes()
        End Function

        Public Function SelectStrokesByNode(node As IInkAnalysisNode) As Rect
            Dim ids = GetNodeStrokeIds(node)
            Dim strokes = GetStrokesByIds(ids)
            Dim rect = SelectStrokes(strokes)
            Return rect
        End Function

        Public Function SelectStrokesByPoints(points As PointCollection) As Rect
            ClearStrokesSelection()
            Dim rect = _strokeContainer.SelectWithPolyLine(points)
            RaiseEvent SelectStrokesEvent(Me, EventArgs.Empty)
            Return rect
        End Function

        Public Sub MoveSelectedStrokes(startPosition As Point, endPosition As Point)
            Dim x = CSng((endPosition.X - startPosition.X))
            Dim y = CSng((endPosition.Y - startPosition.Y))
            Dim matrix = Matrix3x2.CreateTranslation(x, y)

            If Not matrix.IsIdentity Then
                Dim selectedStrokes = GetSelectedStrokes()

                For Each stroke In selectedStrokes
                    stroke.PointTransform *= matrix
                Next

                RaiseEvent MoveStrokesEvent(Me, New MoveStrokesEventArgs(selectedStrokes, startPosition, endPosition))
            End If
        End Sub

        Public Function CopySelectedStrokes() As Rect
            _strokeContainer.CopySelectedToClipboard()
            RaiseEvent CopyStrokesEvent(Me, New CopyPasteStrokesEventArgs(GetSelectedStrokes()))
            Return GetRectBySelectedStrokes()
        End Function

        Public Function CutSelectedStrokes() As Rect
            Dim rect = CopySelectedStrokes()
            Dim selectedStrokes = GetSelectedStrokes().ToList()

            For Each stroke In selectedStrokes
                RemoveStroke(stroke)
            Next

            RaiseEvent CutStrokesEvent(Me, New CopyPasteStrokesEventArgs(selectedStrokes))
            Return rect
        End Function

        Public Function PasteSelectedStrokes(position As Point) As Rect
            Dim rect = Windows.Foundation.Rect.Empty

            If CanPaste Then
                Dim ids = GetStrokes().[Select](Function(s) s.Id).ToList()
                rect = _strokeContainer.PasteFromClipboard(position)
                Dim pastedStrokes = _strokeContainer.GetStrokes().Where(Function(stroke) Not ids.Contains(stroke.Id))
                RaiseEvent PasteStrokesEvent(Me, New CopyPasteStrokesEventArgs(pastedStrokes))
            End If

            Return rect
        End Function

        Public ReadOnly Property CanPaste As Boolean
            Get
                Return _strokeContainer.CanPasteFromClipboard()
            End Get
        End Property

        Public Async Function LoadInkFileAsync(file As StorageFile) As Task(Of Boolean)
            If file Is Nothing Then
                Return False
            End If

            ClearStrokesSelection()
            ClearStrokes()

            Using stream = Await file.OpenSequentialReadAsync()
                Await _strokeContainer.LoadAsync(stream)
            End Using

            RaiseEvent LoadInkFileEvent(Me, EventArgs.Empty)
            Return True
        End Function

        Public Async Function SaveInkFileAsync(file As StorageFile) As Task(Of FileUpdateStatus)
            If file IsNot Nothing Then
                ' Prevent updates to the file until updates are finalized with call to CompleteUpdatesAsync.
                CachedFileManager.DeferUpdates(file)

                Using stream = Await file.OpenAsync(FileAccessMode.ReadWrite)
                    Await _strokeContainer.SaveAsync(stream)
                End Using

                ' Finalize write so other apps can update file.
                Return Await CachedFileManager.CompleteUpdatesAsync(file)
            End If

            Return FileUpdateStatus.Failed
        End Function

        Private Iterator Function GetStrokesByIds(strokeIds As IEnumerable(Of UInteger)) As IEnumerable(Of InkStroke)
            For Each strokeId In strokeIds
                Yield _strokeContainer.GetStrokeById(strokeId)
            Next
        End Function

        Private Function GetNodeStrokeIds(node As IInkAnalysisNode) As IReadOnlyList(Of UInteger)
            Dim strokeIds = node.GetStrokeIds()

            If node.Kind = InkAnalysisNodeKind.Paragraph AndAlso node.Children(0).Kind = InkAnalysisNodeKind.ListItem Then
                strokeIds = New HashSet(Of UInteger)(strokeIds).ToList()
            End If

            Return strokeIds
        End Function

        Private Function GetRectBySelectedStrokes() As Rect
            Dim rect = Windows.Foundation.Rect.Empty

            For Each stroke In GetSelectedStrokes()
                rect.Union(stroke.BoundingRect)
            Next

            Return rect
        End Function
    End Class
End Namespace
