//{[{
using Param_ItemNamespace.Helpers;
using System.Threading.Tasks;
//}]}

namespace Param_ItemNamespace.Views
{
    public sealed partial class wts.ItemNamePage : Page
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
