using Android.App;
using Android.Content.PM;
using XamarinUwpNative.Droid.Services;
using XamarinUwpNative.Services;

[assembly: Xamarin.Forms.Dependency(typeof(PlatformInfoService))]
namespace XamarinUwpNative.Droid.Services
{
    public class PlatformInfoService : IPlatformInfoService
    {
        private static PackageInfo info = Application.Context.PackageManager.GetPackageInfo(
            Application.Context.PackageName,
            PackageInfoFlags.MetaData);

        public string AppName => info.ApplicationInfo.LoadLabel(Application.Context.PackageManager).ToString();

        public string AppVersion => info.VersionName;        
    }
}
