using Microsoft.Templates.UI.Controls;

namespace Microsoft.Templates.UI.ViewModels
{
    public class StatusViewModel
    {
        public StatusType Status { get; set; }
        public string Message { get; set; }
        public bool AutoHide { get; set; }

        public StatusViewModel(StatusType status, string message, bool autoHide = false)
        {
            Status = status;
            Message = message;
            AutoHide = autoHide;
        }
    }
}