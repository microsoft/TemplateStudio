using System.Runtime.InteropServices;
using System.Text;

namespace Param_RootNamespace.Helpers;

public class RuntimeHelper
{
    private const long APPMODEL_ERROR_NO_PACKAGE = 15700L;

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern int GetCurrentPackageFullName(ref int packageFullNameLength, StringBuilder? packageFullName);

    public static bool IsMSIX
    {
        get
        {
            var length = 0;

            return GetCurrentPackageFullName(ref length, null) != APPMODEL_ERROR_NO_PACKAGE;
        }
    }
}
