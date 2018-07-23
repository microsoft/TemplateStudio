//{[{
using Param_ItemNamespace.Helpers;
using System.Threading.Tasks;
//}]}

namespace Param_ItemNamespace.Views
{
    public sealed partial class wts.ItemNamePage : Page
    {
        //{[{
        private bool viewModelinitialized = false;
        //}]}

        public wts.ItemNamePage()
        {
        }

        //{[{
        public async Task OnPivotSelectedAsync()
        {
            if(viewModelinitialized)
            {
                return;
            }

            SetCanvasSize();

            var strokeService = new InkStrokesService(inkCanvas.InkPresenter.StrokeContainer);
            var analyzer = new InkAsyncAnalyzer(inkCanvas, strokeService);
            var selectionRectangleService = new InkSelectionRectangleService(inkCanvas, selectionCanvas, strokeService);

            ViewModel.Initialize(
                strokeService,
                new InkLassoSelectionService(inkCanvas, selectionCanvas, strokeService, selectionRectangleService),
                new InkNodeSelectionService(inkCanvas, selectionCanvas, analyzer, strokeService, selectionRectangleService),
                new InkPointerDeviceService(inkCanvas),
                new InkUndoRedoService(inkCanvas, strokeService),
                new InkTransformService(drawingCanvas, strokeService),
                new InkFileService(inkCanvas, strokeService));

            viewModelinitialized = true;
            await Task.CompletedTask;
        }

        public async Task OnPivotUnselectedAsync()
        {
            await Task.CompletedTask;
        }
        //}]}
    }
}