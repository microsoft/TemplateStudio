using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Windows.UI.Xaml.Controls;

namespace Param_ItemNamespace.Services.Ink
{
    public class InkFileService
    {
        private readonly InkCanvas _inkCanvas;
        private readonly InkStrokesService _strokesService;

        public InkFileService(InkCanvas inkCanvas, InkStrokesService strokesService)
        {
            _inkCanvas = inkCanvas;
            _strokesService = strokesService;
        }

        public async Task<bool> LoadInkAsync()
        {
            var openPicker = new FileOpenPicker
            {
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };

            openPicker.FileTypeFilter.Add(".gif");

            var file = await openPicker.PickSingleFileAsync();
            return await _strokesService.LoadInkFileAsync(file);
        }

        public async Task SaveInkAsync()
        {
            if (!_strokesService.GetStrokes().Any())
            {
                return;
            }

            var savePicker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };

            savePicker.FileTypeChoices.Add("Gif with embedded ISF", new List<string> { ".gif" });

            var file = await savePicker.PickSaveFileAsync();
            await _strokesService.SaveInkFileAsync(file);
        }

        public async Task<StorageFile> ExportToImageAsync(StorageFile imageFile = null)
        {
            if (!_strokesService.GetStrokes().Any())
            {
                return null;
            }

            if (imageFile != null)
            {
                return await ExportCanvasAndImageAsync(imageFile);
            }
            else
            {
                return await ExportCanvasAsync();
            }
        }

        private async Task<StorageFile> ExportCanvasAndImageAsync(StorageFile imageFile)
        {
            var saveFile = await GetImageToSaveAsync();

            if (saveFile == null)
            {
                return null;
            }

            // Prevent updates to the file until updates are finalized with call to CompleteUpdatesAsync.
            CachedFileManager.DeferUpdates(saveFile);

            using (var outStream = await saveFile.OpenAsync(FileAccessMode.ReadWrite))
            {
                var device = CanvasDevice.GetSharedDevice();

                CanvasBitmap canvasbitmap;
                using (var stream = await imageFile.OpenAsync(FileAccessMode.Read))
                {
                    canvasbitmap = await CanvasBitmap.LoadAsync(device, stream);
                }

                using (var renderTarget = new CanvasRenderTarget(device, (int)_inkCanvas.Width, (int)_inkCanvas.Height, canvasbitmap.Dpi))
                {
                    using (CanvasDrawingSession ds = renderTarget.CreateDrawingSession())
                    {
                        ds.DrawImage(canvasbitmap, new Rect(0, 0, (int)_inkCanvas.Width, (int)_inkCanvas.Height));
                        ds.DrawInk(_strokesService.GetStrokes());
                    }

                    await renderTarget.SaveAsync(outStream, CanvasBitmapFileFormat.Png);
                }
            }

            // Finalize write so other apps can update file.
            FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(saveFile);

            return saveFile;
        }

        private async Task<StorageFile> ExportCanvasAsync()
        {
            var file = await GetImageToSaveAsync();

            if (file == null)
            {
                return null;
            }

            CanvasDevice device = CanvasDevice.GetSharedDevice();
            CanvasRenderTarget renderTarget = new CanvasRenderTarget(device, (int)_inkCanvas.Width, (int)_inkCanvas.Height, 96);

            using (var ds = renderTarget.CreateDrawingSession())
            {
                ds.DrawInk(_strokesService.GetStrokes());
            }

            using (var fileStream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                await renderTarget.SaveAsync(fileStream, CanvasBitmapFileFormat.Png);
            }

            return file;
        }

        private async Task<StorageFile> GetImageToSaveAsync()
        {
            var savePicker = new FileSavePicker();
            savePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            savePicker.FileTypeChoices.Add("PNG", new List<string>() { ".png" });
            var saveFile = await savePicker.PickSaveFileAsync();

            return saveFile;
        }
    }
}
