using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Input.Inking;
using Windows.UI.Input.Inking.Analysis;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace InkPoc.Services.Ink
{
    public class InkAsyncAnalyzer
    {
        private readonly InkCanvas inkCanvas;
        private readonly InkStrokesService strokesService;
        private readonly DispatcherTimer dispatcherTimer;
        const double IDLE_WAITING_TIME = 400;
        
        public InkAsyncAnalyzer(InkCanvas _inkCanvas, InkStrokesService _strokesService)
        {
            inkCanvas = _inkCanvas;
            inkCanvas.InkPresenter.StrokeInput.StrokeStarted += (s,e) => StopTimer();
            inkCanvas.InkPresenter.StrokesErased += (s,e) => RemoveStrokes(e.Strokes);
            inkCanvas.InkPresenter.StrokesCollected += (s, e) => AddStrokes(e.Strokes);

            strokesService = _strokesService;
            strokesService.AddStrokeEvent += StrokesService_AddStrokeEvent;
            strokesService.RemoveStrokeEvent += StrokesService_RemoveStrokeEvent;
            strokesService.MoveStrokesEvent += StrokesService_MoveStrokesEvent;
            strokesService.CutStrokesEvent += StrokesService_CutStrokesEvent;
            strokesService.PasteStrokesEvent += StrokesService_PasteStrokesEvent;
            strokesService.ClearStrokesEvent += StrokesService_ClearStrokesEvent;
            strokesService.LoadInkFileEvent += StrokesService_LoadInkFileEvent;

            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(IDLE_WAITING_TIME);
        }
        
        public InkAnalyzer InkAnalyzer { get; private set; } = new InkAnalyzer();

        public bool IsAnalyzing => InkAnalyzer.IsAnalyzing;

        public async Task<bool> AnalyzeAsync(bool clean = false)
        {
            StopTimer();

            if (IsAnalyzing)
            {
                // Ink analyzer is busy. Wait a while and try again.
                StartTimer();
                return false;
            }

            if (clean == true)
            {
                InkAnalyzer.ClearDataForAllStrokes();
                InkAnalyzer.AddDataForStrokes(strokesService.GetStrokes());
            }

            var result = await InkAnalyzer.AnalyzeAsync();
            return result.Status == InkAnalysisStatus.Updated;
        }

        public IInkAnalysisNode FindHitNode(Point position)
        {
            // Start with smallest scope
            var node = FindHitNodeByKind(position, InkAnalysisNodeKind.InkWord);
            if (node == null)
            {
                node = FindHitNodeByKind(position, InkAnalysisNodeKind.InkBullet);
                if (node == null)
                {
                    node = FindHitNodeByKind(position, InkAnalysisNodeKind.InkDrawing);
                }
            }
            return node;
        }

        public void StartTimer() => dispatcherTimer.Start();

        public void StopTimer() => dispatcherTimer.Stop();

        public void ClearAnalysis()
        {
            StopTimer();
            InkAnalyzer.ClearDataForAllStrokes();
        }

        public void AddStroke(InkStroke stroke)
        {
            StopTimer();
            InkAnalyzer.AddDataForStroke(stroke);
            StartTimer();
        }

        public void AddStrokes(IReadOnlyList<InkStroke> strokes)
        {
            StopTimer();
            InkAnalyzer.AddDataForStrokes(strokes);
            StartTimer();
        }

        public void RemoveStroke(InkStroke stroke)
        {
            StopTimer();
            InkAnalyzer.RemoveDataForStroke(stroke.Id);
            StartTimer();
        }

        public void RemoveStrokes(IReadOnlyList<InkStroke> strokes)
        {
            StopTimer();

            foreach (var stroke in strokes)
            {
                // Remove strokes from InkAnalyzer
                InkAnalyzer.RemoveDataForStroke(stroke.Id);
            }

            StartTimer();
        }

        public void ReplaceStroke(InkStroke stroke)
        {
            InkAnalyzer.ReplaceDataForStroke(stroke);
        }

        private async void DispatcherTimer_Tick(object sender, object e) => await AnalyzeAsync();

        private IInkAnalysisNode FindHitNodeByKind(Point position, InkAnalysisNodeKind kind)
        {
            var nodes = InkAnalyzer.AnalysisRoot.FindNodes(kind);
            foreach (var node in nodes)
            {
                if (RectHelper.Contains(node.BoundingRect, position))
                {
                    return node;
                }
            }
            return null;
        }

        private void StrokesService_AddStrokeEvent(object sender, AddStrokeEventArgs e)
        {
            AddStroke(e.NewStroke);
        }

        private void StrokesService_RemoveStrokeEvent(object sender, RemoveEventArgs e)
        {
            RemoveStroke(e.RemovedStroke);
        }

        private async void StrokesService_MoveStrokesEvent(object sender, MoveStrokesEventArgs e)
        {
            foreach (var stroke in e.Strokes)
            {
                ReplaceStroke(stroke);
            }

            // Strokes are moved and the analysis result is not valid anymore.
            await AnalyzeAsync(true);
        }

        private void StrokesService_PasteStrokesEvent(object sender, CopyPasteStrokesEventArgs e)
        {
            foreach (var stroke in e.Strokes)
            {
                AddStroke(stroke);
            }
        }

        private void StrokesService_CutStrokesEvent(object sender, CopyPasteStrokesEventArgs e)
        {
            foreach(var stroke in e.Strokes)
            {
                RemoveStroke(stroke);
            }
        }

        private void StrokesService_ClearStrokesEvent(object sender, EventArgs e)
        {
            ClearAnalysis();
        }


        private async void StrokesService_LoadInkFileEvent(object sender, EventArgs e)
        {
            await AnalyzeAsync(true);
        }
    }
}
