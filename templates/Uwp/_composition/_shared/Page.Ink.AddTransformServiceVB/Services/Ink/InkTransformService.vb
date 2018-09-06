Imports Windows.UI
Imports Windows.UI.Input.Inking
Imports Windows.UI.Input.Inking.Analysis
Imports Windows.UI.Xaml.Shapes

Namespace Services.Ink
    Public Class InkTransformService
        Private ReadOnly _inkAnalyzer As InkAnalyzer
        Private ReadOnly _drawingCanvas As Canvas
        Private ReadOnly _strokeService As InkStrokesService

        Public Sub New(drawingCanvas As Canvas, strokeService As InkStrokesService)
            _drawingCanvas = drawingCanvas
            _strokeService = strokeService
            _inkAnalyzer = New InkAnalyzer()
        End Sub

        Public Async Function TransformTextAndShapesAsync() As Task(Of InkTransformResult)
            Dim result = New InkTransformResult(_drawingCanvas)
            Dim inkStrokes = GetStrokesToConvert()

            If inkStrokes.Any() Then
                _inkAnalyzer.ClearDataForAllStrokes()
                _inkAnalyzer.AddDataForStrokes(inkStrokes)
                Dim inkAnalysisResults = Await _inkAnalyzer.AnalyzeAsync()

                If inkAnalysisResults.Status = InkAnalysisStatus.Updated Then
                    Dim words = AnalyzeWords()
                    Dim shapes = AnalyzeShapes()

                    ' Generate result
                    result.Strokes.AddRange(inkStrokes)
                    result.TextAndShapes.AddRange(words)
                    result.TextAndShapes.AddRange(shapes)
                End If
            End If

            Return result
        End Function

        Public Function GetTextAndShapes() As IEnumerable(Of UIElement)
            Return _drawingCanvas.Children
        End Function

        Public Sub AddUIElement(element As UIElement)
            _drawingCanvas.Children.Add(element)
        End Sub

        Public Sub RemoveUIElement(element As UIElement)
            If _drawingCanvas.Children.Contains(element) Then
                _drawingCanvas.Children.Remove(element)
            End If
        End Sub

        Public Function HasTextAndShapes as Boolean
            Return _drawingCanvas.Children.Any()
        End Function

        Public Sub ClearTextAndShapes()
            _drawingCanvas.Children.Clear()
        End Sub

        Private Iterator Function AnalyzeWords() As IEnumerable(Of UIElement)
            Dim inkwordNodes = _inkAnalyzer.AnalysisRoot.FindNodes(InkAnalysisNodeKind.InkWord)

            For Each node As InkAnalysisInkWord In inkwordNodes
                Dim textblock = DrawText(node.RecognizedText, node.BoundingRect)
                Dim strokesIds = node.GetStrokeIds()
                _strokeService.RemoveStrokesByIds(strokesIds)
                _inkAnalyzer.RemoveDataForStrokes(strokesIds)
                Yield textblock
            Next
        End Function

        Private Iterator Function AnalyzeShapes() As IEnumerable(Of UIElement)
            Dim inkdrawingNodes = _inkAnalyzer.AnalysisRoot.FindNodes(InkAnalysisNodeKind.InkDrawing)

            For Each node As InkAnalysisInkDrawing In inkdrawingNodes
                Dim strokesIds = node.GetStrokeIds()

                If node.DrawingKind = InkAnalysisDrawingKind.Drawing Then
                    ' Catch and process unsupported shapes (lines and so on) here.
                Else

                    If node.DrawingKind = InkAnalysisDrawingKind.Circle OrElse node.DrawingKind = InkAnalysisDrawingKind.Ellipse Then
                        Yield DrawEllipse(node)
                    Else
                        Yield DrawPolygon(node)
                    End If

                    _strokeService.RemoveStrokesByIds(strokesIds)
                End If

                _inkAnalyzer.RemoveDataForStrokes(strokesIds)
            Next
        End Function

        Private Function DrawText(recognizedText As String, boundingRect As Rect) As UIElement
            Dim text As TextBlock = New TextBlock()
            Canvas.SetTop(text, boundingRect.Top)
            Canvas.SetLeft(text, boundingRect.Left)
            text.Text = recognizedText
            text.FontSize = boundingRect.Height
            text.Foreground = New SolidColorBrush(Colors.Black)
            _drawingCanvas.Children.Add(text)
            Return text
        End Function

        Private Function DrawEllipse(shape As InkAnalysisInkDrawing) As UIElement
            Dim points = shape.Points
            Dim ellipse As Ellipse = New Ellipse()
            ellipse.Width = Math.Sqrt(((points(0).X - points(2).X) * (points(0).X - points(2).X)) + ((points(0).Y - points(2).Y) * (points(0).Y - points(2).Y)))
            ellipse.Height = Math.Sqrt(((points(1).X - points(3).X) * (points(1).X - points(3).X)) + ((points(1).Y - points(3).Y) * (points(1).Y - points(3).Y)))
            Dim rotAngle = Math.Atan2(points(2).Y - points(0).Y, points(2).X - points(0).X)
            Dim rotateTransform As RotateTransform = New RotateTransform()
            rotateTransform.Angle = rotAngle * 180 / Math.PI
            rotateTransform.CenterX = ellipse.Width / 2.0
            rotateTransform.CenterY = ellipse.Height / 2.0
            Dim translateTransform As TranslateTransform = New TranslateTransform()
            translateTransform.X = shape.Center.X - (ellipse.Width / 2.0)
            translateTransform.Y = shape.Center.Y - (ellipse.Height / 2.0)
            Dim transformGroup As TransformGroup = New TransformGroup()
            transformGroup.Children.Add(rotateTransform)
            transformGroup.Children.Add(translateTransform)
            ellipse.RenderTransform = transformGroup
            Dim brush = New SolidColorBrush(ColorHelper.FromArgb(255, 0, 0, 255))
            ellipse.Stroke = brush
            ellipse.StrokeThickness = 2
            _drawingCanvas.Children.Add(ellipse)
            Return ellipse
        End Function

        Private Function DrawPolygon(shape As InkAnalysisInkDrawing) As UIElement
            Dim points = shape.Points
            Dim polygon As Polygon = New Polygon()

            For Each point In points
                polygon.Points.Add(point)
            Next

            Dim brush = New SolidColorBrush(ColorHelper.FromArgb(255, 0, 0, 255))
            polygon.Stroke = brush
            polygon.StrokeThickness = 2
            _drawingCanvas.Children.Add(polygon)
            Return polygon
        End Function

        Private Function GetStrokesToConvert() As IEnumerable(Of InkStroke)
            Dim selectedStrokes = _strokeService.GetSelectedStrokes()

            If selectedStrokes.Any() Then
                Return selectedStrokes
            End If

            Return _strokeService.GetStrokes()
        End Function
    End Class
End Namespace
