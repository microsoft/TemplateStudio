//{[{
using System.Diagnostics;
//}]}
namespace Param_RootNamespace.Services
{
    public class SystemService : ISystemService
    {
//^^
//{[{
        public void OpenInWebBrowser(string url)
        {
            // For more info see https://github.com/dotnet/corefx/issues/10361
            var psi = new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            };
            Process.Start(psi);
        }
//}]}
    }
}