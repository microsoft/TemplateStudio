using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Provider;
using Windows.UI.Input.Inking;
using Windows.UI.Input.Inking.Analysis;
using Windows.UI.Xaml.Media;
using Param_ItemNamespace.EventHandlers.Ink;

namespace Param_ItemNamespace.Services.Ink
{
    public class InkStrokesService
    {
        private readonly InkStrokeContainer _strokeContainer;

        public InkStrokesService(InkStrokeContainer strokeContainer)
        {
            _strokeContainer = strokeContainer;
        }

        public event EventHandler<AddStrokeEventArgs> AddStrokeEvent;

        public event EventHandler<RemoveEventArgs> RemoveStrokeEvent;

        public event EventHandler<MoveStrokesEventArgs> MoveStrokesEvent;

        public event EventHandler<EventArgs> ClearStrokesEvent;

        public event EventHandler<CopyPasteStrokesEventArgs> CutStrokesEvent;

        public event EventHandler<CopyPasteStrokesEventArgs> PasteStrokesEvent;

        public event EventHandler<EventArgs> LoadInkFileEvent;

        public InkStroke AddStroke(InkStroke stroke)
        {
            var newStroke = stroke.Clone();
            _strokeContainer.AddStroke(newStroke);

            AddStrokeEvent?.Invoke(this, new AddStrokeEventArgs(newStroke, stroke));

            return newStroke;
        }

        public bool RemoveStroke(InkStroke stroke)
        {
            var deleteStroke = GetStrokes().FirstOrDefault(s => s.Id == stroke.Id);
            if (deleteStroke == null)
            {
                return false;
            }

            ClearStrokesSelection();
            deleteStroke.Selected = true;
            _strokeContainer.DeleteSelected();

            RemoveStrokeEvent?.Invoke(this, new RemoveEventArgs(stroke));
            return true;
        }

        public bool RemoveStrokesByIds(IEnumerable<uint> strokeIds)
        {
            var strokes = GetStrokesByIds(strokeIds);

            foreach (var stroke in strokes)
            {
                RemoveStroke(stroke);
            }

            return strokes.Any();
        }

        public IEnumerable<InkStroke> GetStrokes() => _strokeContainer.GetStrokes();

        public IEnumerable<InkStroke> GetSelectedStrokes() => GetStrokes().Where(s => s.Selected);

        public void ClearStrokes()
        {
            _strokeContainer.Clear();
            ClearStrokesEvent?.Invoke(this, EventArgs.Empty);
        }

        public void ClearStrokesSelection()
        {
            foreach (var stroke in GetStrokes())
            {
                stroke.Selected = false;
            }
        }

        public Rect SelectStrokes(IEnumerable<InkStroke> strokes)
        {
            ClearStrokesSelection();

            foreach (var stroke in strokes)
            {
                stroke.Selected = true;
            }

            return GetRectBySelectedStrokes();
        }

        public Rect SelectStrokesByNode(IInkAnalysisNode node)
        {
            var ids = GetNodeStrokeIds(node);
            var strokes = GetStrokesByIds(ids);
            var rect = SelectStrokes(strokes);

            return rect;
        }

        public Rect SelectStrokesByPoints(PointCollection points)
        {
            ClearStrokesSelection();
            return _strokeContainer.SelectWithPolyLine(points);
        }

        public void MoveSelectedStrokes(Point startPosition, Point endPosition)
        {
            var x = (float)(endPosition.X - startPosition.X);
            var y = (float)(endPosition.Y - startPosition.Y);

            var matrix = Matrix3x2.CreateTranslation(x, y);

            if (!matrix.IsIdentity)
            {
                var selectedStrokes = GetSelectedStrokes();
                foreach (var stroke in selectedStrokes)
                {
                    stroke.PointTransform *= matrix;
                }

                MoveStrokesEvent?.Invoke(this, new MoveStrokesEventArgs(selectedStrokes, startPosition, endPosition));
            }
        }

        public Rect CopySelectedStrokes()
        {
            _strokeContainer.CopySelectedToClipboard();
            return GetRectBySelectedStrokes();
        }

        public Rect CutSelectedStrokes()
        {
            var rect = CopySelectedStrokes();

            var selectedStrokes = GetSelectedStrokes().ToList();

            foreach (var stroke in selectedStrokes)
            {
                RemoveStroke(stroke);
            }

            CutStrokesEvent?.Invoke(this, new CopyPasteStrokesEventArgs(selectedStrokes));

            return rect;
        }

        public Rect PasteSelectedStrokes(Point position)
        {
            var rect = Rect.Empty;

            if (CanPaste)
            {
                var ids = GetStrokes().Select(s => s.Id).ToList();

                rect = _strokeContainer.PasteFromClipboard(position);

                var pastedStrokes = _strokeContainer.GetStrokes().Where(s => !ids.Contains(s.Id));

                PasteStrokesEvent?.Invoke(this, new CopyPasteStrokesEventArgs(pastedStrokes));
            }

            return rect;
        }

        public bool CanPaste => _strokeContainer.CanPasteFromClipboard();

        public async Task<bool> LoadInkFileAsync(StorageFile file)
        {
            if (file == null)
            {
                return false;
            }

            ClearStrokesSelection();
            ClearStrokes();

            using (var stream = await file.OpenSequentialReadAsync())
            {
                await _strokeContainer.LoadAsync(stream);
            }

            LoadInkFileEvent?.Invoke(this, EventArgs.Empty);
            return true;
        }

        public async Task<FileUpdateStatus> SaveInkFileAsync(StorageFile file)
        {
            if (file != null)
            {
                // Prevent updates to the file until updates are finalized with call to CompleteUpdatesAsync.
                CachedFileManager.DeferUpdates(file);

                using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    await _strokeContainer.SaveAsync(stream);
                }

                // Finalize write so other apps can update file.
                return await CachedFileManager.CompleteUpdatesAsync(file);
            }

            return FileUpdateStatus.Failed;
        }

        private IEnumerable<InkStroke> GetStrokesByIds(IEnumerable<uint> strokeIds)
        {
            foreach (var strokeId in strokeIds)
            {
                yield return _strokeContainer.GetStrokeById(strokeId);
            }
        }

        private IReadOnlyList<uint> GetNodeStrokeIds(IInkAnalysisNode node)
        {
            var strokeIds = node.GetStrokeIds();
            if (node.Kind == InkAnalysisNodeKind.Paragraph && node.Children[0].Kind == InkAnalysisNodeKind.ListItem)
            {
                strokeIds = new HashSet<uint>(strokeIds).ToList();
            }

            return strokeIds;
        }

        private Rect GetRectBySelectedStrokes()
        {
            var rect = Rect.Empty;
            foreach (var stroke in GetSelectedStrokes())
            {
                rect.Union(stroke.BoundingRect);
            }

            return rect;
        }
    }
}
