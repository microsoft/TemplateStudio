﻿//{[{
using Windows.ApplicationModel;
using OSVersionHelper;
//}]}
namespace Param_RootNamespace.Services;

public class ApplicationInfoService : IApplicationInfoService
{
    public Version GetVersion()
    {
//{[{
        if (WindowsVersionHelper.HasPackageIdentity)
        {
            // Packaged application
            // Set the app version in Param_ProjectName.Packaging > Package.appxmanifest > Packaging > PackageVersion
            var packageVersion = Package.Current.Id.Version;
            return new Version(packageVersion.Major, packageVersion.Minor, packageVersion.Build, packageVersion.Revision);
        }
//}]}
    }
}
