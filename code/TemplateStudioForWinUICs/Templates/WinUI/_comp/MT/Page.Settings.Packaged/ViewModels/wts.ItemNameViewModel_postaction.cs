//{[{
using Windows.ApplicationModel;
//}]}

        private string GetVersionDescription()
        {
            //{[{
            var appName = "AppDisplayName".GetLocalized();
            var version = Package.Current.Id.Version;

            return $"{appName} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
            //}]}
        }