using Windows.ApplicationModel;
using WtsXamarin.Services;
using WtsXamarin.UWP.Services;

[assembly: Xamarin.Forms.Dependency(typeof(PlatformInfoService))]
namespace WtsXamarin.UWP.Services
{
    public class PlatformInfoService : IPlatformInfoService
    {
        public string AppName => Package.Current.DisplayName;

        public string AppVersion => GetAppVersion();
        
        private string GetAppVersion()
        {
            var package = Package.Current;
            var packageId = package.Id;
            var version = packageId.Version;
            return $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }
    }
}
