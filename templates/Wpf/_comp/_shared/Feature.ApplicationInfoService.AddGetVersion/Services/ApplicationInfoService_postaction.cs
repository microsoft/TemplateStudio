//{[{
using System.Diagnostics;
using System.Reflection;
//}]}
namespace Param_RootNamespace.Services
{
    public class ApplicationInfoService : IApplicationInfoService
    {
//^^
//{[{
        public Version GetVersion()
        {
            //// Setup the App Version in Param_ProjectName > Properties > Package > PackageVersion
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            var version = FileVersionInfo.GetVersionInfo(assemblyLocation).FileVersion;
            return new Version(version);
        }
//}]}
    }
}