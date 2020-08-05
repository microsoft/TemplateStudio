Imports Microsoft.Graphics.Canvas
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Threading.Tasks
Imports Windows.Foundation
Imports Windows.Storage
Imports Windows.Storage.Pickers
Imports Windows.Storage.Provider

Namespace Services.Ink
    Public Class InkFileService
        Private ReadOnly _inkCanvas As InkCanvas
        Private ReadOnly _strokesService As InkStrokesService

        Public Sub New(inkCanvas As InkCanvas, strokesService As InkStrokesService)
            _inkCanvas = inkCanvas
            _strokesService = strokesService
        End Sub

        Public Async Function LoadInkAsync() As Task(Of Boolean)
            Dim openPicker = New FileOpenPicker With {
                .SuggestedStartLocation = PickerLocationId.PicturesLibrary
            }
            openPicker.FileTypeFilter.Add(".gif")
            Dim file = Await openPicker.PickSingleFileAsync()
            Return Await _strokesService.LoadInkFileAsync(file)
        End Function

        Public Async Function SaveInkAsync() As Task
            If Not _strokesService.GetStrokes().Any() Then
                Return
            End If

            Dim savePicker = New FileSavePicker With {
                .SuggestedStartLocation = PickerLocationId.PicturesLibrary
            }
            savePicker.FileTypeChoices.Add("Gif with embedded ISF", New List(Of String) From {
                ".gif"
            })
            Dim file = Await savePicker.PickSaveFileAsync()
            Await _strokesService.SaveInkFileAsync(file)
        End Function

        Public Async Function ExportToImageAsync(Optional imageFile As StorageFile = Nothing) As Task(Of StorageFile)
            If Not _strokesService.GetStrokes().Any() Then
                Return Nothing
            End If

            If imageFile IsNot Nothing Then
                Return Await ExportCanvasAndImageAsync(imageFile)
            Else
                Return Await ExportCanvasAsync()
            End If
        End Function

        Private Async Function ExportCanvasAndImageAsync(imageFile As StorageFile) As Task(Of StorageFile)
            Dim saveFile = Await GetImageToSaveAsync()

            If saveFile Is Nothing Then
                Return Nothing
            End If

            ' Prevent updates to the file until updates are finalized with call to CompleteUpdatesAsync.
            CachedFileManager.DeferUpdates(saveFile)

            Using outStream = Await saveFile.OpenAsync(FileAccessMode.ReadWrite)
                Dim device = CanvasDevice.GetSharedDevice()
                Dim canvasbitmap As CanvasBitmap

                Using stream = Await imageFile.OpenAsync(FileAccessMode.Read)
                    canvasbitmap = Await CanvasBitmap.LoadAsync(device, stream)
                End Using

                Using renderTarget = New CanvasRenderTarget(device, CInt(_inkCanvas.Width), CInt(_inkCanvas.Height), canvasbitmap.Dpi)

                    Using ds As CanvasDrawingSession = renderTarget.CreateDrawingSession()
                        ds.DrawImage(canvasbitmap, New Rect(0, 0, CInt(_inkCanvas.Width), CInt(_inkCanvas.Height)))
                        ds.DrawInk(_strokesService.GetStrokes())
                    End Using

                    Await renderTarget.SaveAsync(outStream, CanvasBitmapFileFormat.Png)
                End Using
            End Using

            '  Finalize write so other apps can update file.
            Await CachedFileManager.CompleteUpdatesAsync(saveFile)
            Return saveFile
        End Function

        Private Async Function ExportCanvasAsync() As Task(Of StorageFile)
            Dim file = Await GetImageToSaveAsync()

            If file Is Nothing Then
                Return Nothing
            End If

            Dim device As CanvasDevice = CanvasDevice.GetSharedDevice()
            Dim renderTarget As CanvasRenderTarget = New CanvasRenderTarget(device, CInt(_inkCanvas.Width), CInt(_inkCanvas.Height), 96)

            Using ds = renderTarget.CreateDrawingSession()
                ds.DrawInk(_strokesService.GetStrokes())
            End Using

            Using fileStream = Await file.OpenAsync(FileAccessMode.ReadWrite)
                Await renderTarget.SaveAsync(fileStream, CanvasBitmapFileFormat.Png)
            End Using

            Return file
        End Function

        Private Async Function GetImageToSaveAsync() As Task(Of StorageFile)
            Dim savePicker = New FileSavePicker()
            savePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary
            savePicker.FileTypeChoices.Add("PNG", New List(Of String)() From {
                ".png"
            })
            Dim saveFile = Await savePicker.PickSaveFileAsync()
            Return saveFile
        End Function
    End Class
End Namespace
