namespace Param_RootNamespace.Services.DragAndDrop
{
    public class DragDropService
    {
        private static void ConfigureUIElement(UIElement element, DropConfiguration configuration)
        {
            //{[{
            element.DragEnter += (sender, args) =>
            {
                // Operation is copy by default
                args.AcceptedOperation = DataPackageOperation.Copy;

                var data = new DragDropData { AcceptedOperation = args.AcceptedOperation, DataView = args.DataView };
                configuration.DragEnterCommand?.Execute(data);
                args.AcceptedOperation = data.AcceptedOperation;
            };

            element.DragOver += (sender, args) =>
            {
                var data = new DragDropData { AcceptedOperation = args.AcceptedOperation, DataView = args.DataView };
                configuration.DragOverCommand?.Execute(data);
                args.AcceptedOperation = data.AcceptedOperation;
            };

            element.DragLeave += (sender, args) =>
            {
                var data = new DragDropData { AcceptedOperation = args.AcceptedOperation, DataView = args.DataView };
                configuration.DragLeaveCommand?.Execute(data);
            };

            element.Drop += async (sender, args) =>
            {
                await configuration.ProcessComandsAsync(args.DataView);
            };
            //}]}
        }

        private static void ConfigureListView(ListViewBase listview, ListViewDropConfiguration configuration)
        {
            //{[{
            listview.DragItemsStarting += (sender, args) =>
            {
                var data = new DragDropStartingData { Data = args.Data, Items = args.Items };
                configuration.DragItemsStartingCommand?.Execute(data);
            };

            listview.DragItemsCompleted += (sender, args) =>
            {
                var data = new DragDropCompletedData { DropResult = args.DropResult, Items = args.Items };
                configuration.DragItemsCompletedCommand?.Execute(data);
            };
            //}]}
        }
    }
}
