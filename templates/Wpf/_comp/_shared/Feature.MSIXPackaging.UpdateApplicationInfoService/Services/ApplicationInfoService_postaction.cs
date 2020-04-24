//{[{
using Windows.ApplicationModel;
using OSVersionHelper;
//}]}
namespace Param_RootNamespace.Services
{
    public class ApplicationInfoService : IApplicationInfoService
    {
        public Version GetVersion()
        {
//{[{
            if (WindowsVersionHelper.HasPackageIdentity)
            {
                //// MSIX distribuition
                //// Setup the App Version in Param_ProjectName.Packaging > Package.appxmanifest > Packaging > PackageVersion
                var version = Package.Current.Id.Version;
                return new Version(version.Major, version.Minor, version.Build, version.Revision);
            }
//}]}
        }
    }
}
