//{[{
using System.Reflection;
using Windows.ApplicationModel;
using Windows.Storage;
//}]}

private static string GetVersionDescription()
    {
        //{[{
        private readonly bool _isPackaged;
        var appName = "AppDisplayName".GetLocalized();
        try
        {
            var package = Package.Current;
            _isPackaged = true;
        }
        catch (InvalidOperationException)
        {
            _isPackaged = false;
        }

        if (_isPackaged)
        {
            var version = Package.Current.Id.Version;
            return $"{appName} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }
        else
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            return $"{appName} - {version?.Major}.{version?.Minor}.{version?.Build}.{version?.Revision}";
        }
        //}]}
    }
