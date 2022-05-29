//{[{
using System.Reflection;
//}]}

        private static string GetVersionDescription()
        {
            //{[{
            var appName = "AppDisplayName".GetLocalized();
            var version = Assembly.GetExecutingAssembly().GetName().Version;

            return $"{appName} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
            //}]}
        }
