using Foundation;
using XamarinUwpNative.iOS.Services;
using XamarinUwpNative.Services;

[assembly: Xamarin.Forms.Dependency(typeof(PlatformInfoService))]
namespace XamarinUwpNative.iOS.Services
{
    public class PlatformInfoService : IPlatformInfoService
    {
        public string AppName => NSBundle.MainBundle.InfoDictionary["CFBundleName"].ToString();

        public string AppVersion => NSBundle.MainBundle.InfoDictionary["CFBundleShortVersionString"].ToString();
    }
}
