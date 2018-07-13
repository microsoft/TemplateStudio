//{[{
using System.Threading.Tasks;
using Param_ItemNamespace.Helpers;
//}]}

namespace Param_ItemNamespace.Views
{
    public sealed partial class wts.ItemNamePage : Page, INotifyPropertyChanged
    {
        public wts.ItemNamePage()
        {
        }

        //{[{
        public async Task OnPivotSelectedAsync()
        {
            await cameraControl.InitializeCameraAsync();
        }

        public async Task OnPivotUnselectedAsync()
        {
            await cameraControl.CleanupCameraAsync();
        }
        //}]}
    }
}