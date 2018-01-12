using Foundation;
using Wts.iOS.Services;
using Wts.Mobile.Services;

[assembly: Xamarin.Forms.Dependency(typeof(PlatformInfoService))]
namespace Wts.iOS.Services
{
    public class PlatformInfoService : IPlatformInfoService
    {
        public string AppName => NSBundle.MainBundle.InfoDictionary["CFBundleName"].ToString();

        public string AppVersion => NSBundle.MainBundle.InfoDictionary["CFBundleShortVersionString"].ToString();
    }
}
