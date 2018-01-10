using Foundation;
using WtsXamarinUWP.iOS.Services;
using WtsXamarinUWP.Mobile.Services;

[assembly: Xamarin.Forms.Dependency(typeof(PlatformInfoService))]
namespace WtsXamarinUWP.iOS.Services
{
    public class PlatformInfoService : IPlatformInfoService
    {
        public string AppName => NSBundle.MainBundle.InfoDictionary["CFBundleName"].ToString();

        public string AppVersion => NSBundle.MainBundle.InfoDictionary["CFBundleShortVersionString"].ToString();
    }
}
