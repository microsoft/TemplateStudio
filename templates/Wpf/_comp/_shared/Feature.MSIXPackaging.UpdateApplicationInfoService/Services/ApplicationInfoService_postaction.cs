//{--{
using System.Diagnostics;
using System.Reflection;
//}--}
//{[{
using Windows.ApplicationModel;
//}]}
namespace Param_RootNamespace.Services
{
    public class ApplicationInfoService : IApplicationInfoService
    {
        public Version GetVersion()
        {
//{--{
            //// Setup the App Version in Param_ProjectName > Properties > Package > PackageVersion
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            var version = FileVersionInfo.GetVersionInfo(assemblyLocation).FileVersion;
            return new Version(version);
//}--}
//{[{
            //// MSIX distribuition
            //// Setup the App Version in Param_ProjectName.Packaging > Package.appxmanifest > Packaging > PackageVersion
            var version = Package.Current.Id.Version;
            return new Version(version.Major, version.Minor, version.Build, version.Revision);
//}]}
        }
    }
}