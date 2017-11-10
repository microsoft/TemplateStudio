using Windows.ApplicationModel.DataTransfer;

namespace DragAndDropExample.Models
{
    public class DragDropData
    { 
        public DataPackageOperation AcceptedOperation { get; set; }

        public DataPackageView DataView { get; set; }
    }
}
