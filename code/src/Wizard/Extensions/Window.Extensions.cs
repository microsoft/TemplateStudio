using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Microsoft.Templates.Wizard
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
