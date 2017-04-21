using System;
using System.Windows;

namespace Microsoft.Templates.UI
{
    public static class WindowExtensions
    {
        public static void SafeClose(this Window window)
        {
            try
            {
                window?.Close();
            }
            catch (Exception)
            {
            }
        }
    }
}
