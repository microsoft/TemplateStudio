//{[{
using Windows.ApplicationModel;
//}]}

    private static string GetVersionDescription()
    {
        //{[{
        var appName = "AppDisplayName".GetLocalized();
        var version = Package.Current.Id.Version;

        return $"{appName} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        //}]}
    }
