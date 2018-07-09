using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Input.Inking;
using Windows.UI.Input.Inking.Analysis;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Param_ItemNamespace.Services.Ink
{
    public class InkTransformService
    {
        private readonly InkAnalyzer _inkAnalyzer;
        private readonly Canvas _drawingCanvas;
        private readonly InkStrokesService _strokeService;

        public InkTransformService(Canvas drawingCanvas, InkStrokesService strokeService)
        {
            _drawingCanvas = drawingCanvas;
            _strokeService = strokeService;
            _inkAnalyzer = new InkAnalyzer();
        }

        public async Task<InkTransformResult> TransformTextAndShapesAsync()
        {
            var result = new InkTransformResult(_drawingCanvas);
            var inkStrokes = GetStrokesToConvert();

            if (inkStrokes.Any())
            {
                _inkAnalyzer.ClearDataForAllStrokes();
                _inkAnalyzer.AddDataForStrokes(inkStrokes);
                var inkAnalysisResults = await _inkAnalyzer.AnalyzeAsync();

                if (inkAnalysisResults.Status == InkAnalysisStatus.Updated)
                {
                    var words = AnalyzeWords();
                    var shapes = AnalyzeShapes();

                    // Generate result
                    result.Strokes.AddRange(inkStrokes);
                    result.TextAndShapes.AddRange(words);
                    result.TextAndShapes.AddRange(shapes);
                }
            }

            return result;
        }

        public void ClearTextAndShapes()
        {
            _drawingCanvas.Children.Clear();
        }

        private IEnumerable<UIElement> AnalyzeWords()
        {
            var inkwordNodes = _inkAnalyzer.AnalysisRoot.FindNodes(InkAnalysisNodeKind.InkWord);

            foreach (InkAnalysisInkWord node in inkwordNodes)
            {
                var textblock = DrawText(node.RecognizedText, node.BoundingRect);

                var strokesIds = node.GetStrokeIds();
                _strokeService.RemoveStrokesByIds(strokesIds);
                _inkAnalyzer.RemoveDataForStrokes(strokesIds);

                yield return textblock;
            }
        }

        private IEnumerable<UIElement> AnalyzeShapes()
        {
            var inkdrawingNodes = _inkAnalyzer.AnalysisRoot.FindNodes(InkAnalysisNodeKind.InkDrawing);
            foreach (InkAnalysisInkDrawing node in inkdrawingNodes)
            {
                var strokesIds = node.GetStrokeIds();

                if (node.DrawingKind == InkAnalysisDrawingKind.Drawing)
                {
                    // Catch and process unsupported shapes (lines and so on) here.
                }
                else
                {
                    if (node.DrawingKind == InkAnalysisDrawingKind.Circle || node.DrawingKind == InkAnalysisDrawingKind.Ellipse)
                    {
                        yield return DrawEllipse(node);
                    }
                    else
                    {
                        yield return DrawPolygon(node);
                    }

                    _strokeService.RemoveStrokesByIds(strokesIds);
                }

                _inkAnalyzer.RemoveDataForStrokes(strokesIds);
            }
        }

        private UIElement DrawText(string recognizedText, Rect boundingRect)
        {
            TextBlock text = new TextBlock();
            Canvas.SetTop(text, boundingRect.Top);
            Canvas.SetLeft(text, boundingRect.Left);

            text.Text = recognizedText;
            text.FontSize = boundingRect.Height;
            text.Foreground = new SolidColorBrush(Colors.Black);

            _drawingCanvas.Children.Add(text);
            return text;
        }

        private UIElement DrawEllipse(InkAnalysisInkDrawing shape)
        {
            var points = shape.Points;
            Ellipse ellipse = new Ellipse();
            ellipse.Width = Math.Sqrt(((points[0].X - points[2].X) * (points[0].X - points[2].X)) +
                 ((points[0].Y - points[2].Y) * (points[0].Y - points[2].Y)));
            ellipse.Height = Math.Sqrt(((points[1].X - points[3].X) * (points[1].X - points[3].X)) +
                 ((points[1].Y - points[3].Y) * (points[1].Y - points[3].Y)));

            var rotAngle = Math.Atan2(points[2].Y - points[0].Y, points[2].X - points[0].X);
            RotateTransform rotateTransform = new RotateTransform();
            rotateTransform.Angle = rotAngle * 180 / Math.PI;
            rotateTransform.CenterX = ellipse.Width / 2.0;
            rotateTransform.CenterY = ellipse.Height / 2.0;

            TranslateTransform translateTransform = new TranslateTransform();
            translateTransform.X = shape.Center.X - (ellipse.Width / 2.0);
            translateTransform.Y = shape.Center.Y - (ellipse.Height / 2.0);

            TransformGroup transformGroup = new TransformGroup();
            transformGroup.Children.Add(rotateTransform);
            transformGroup.Children.Add(translateTransform);
            ellipse.RenderTransform = transformGroup;

            var brush = new SolidColorBrush(ColorHelper.FromArgb(255, 0, 0, 255));
            ellipse.Stroke = brush;
            ellipse.StrokeThickness = 2;
            _drawingCanvas.Children.Add(ellipse);

            return ellipse;
        }

        private UIElement DrawPolygon(InkAnalysisInkDrawing shape)
        {
            var points = shape.Points;
            Polygon polygon = new Polygon();

            foreach (var point in points)
            {
                polygon.Points.Add(point);
            }

            var brush = new SolidColorBrush(Windows.UI.ColorHelper.FromArgb(255, 0, 0, 255));
            polygon.Stroke = brush;
            polygon.StrokeThickness = 2;
            _drawingCanvas.Children.Add(polygon);

            return polygon;
        }

        private IEnumerable<InkStroke> GetStrokesToConvert()
        {
            var selectedStrokes = _strokeService.GetSelectedStrokes();

            if (selectedStrokes.Any())
            {
                return selectedStrokes;
            }

            return _strokeService.GetStrokes();
        }
    }
}