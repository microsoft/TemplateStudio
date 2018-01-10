using Foundation;
using WtsXPlat.iOS.Services;
using WtsXPlat.Mobile.Services;

[assembly: Xamarin.Forms.Dependency(typeof(PlatformInfoService))]
namespace WtsXPlat.iOS.Services
{
    public class PlatformInfoService : IPlatformInfoService
    {
        public string AppName => NSBundle.MainBundle.InfoDictionary["CFBundleName"].ToString();

        public string AppVersion => NSBundle.MainBundle.InfoDictionary["CFBundleShortVersionString"].ToString();
    }
}
