using Windows.ApplicationModel.DataTransfer;

namespace DragAndDropExample.Models
{
    public class DragOverData
    { 
        public DataPackageOperation AcceptedOperation { get; set; }

        public DataPackageView DataView { get; set; }
    }
}
