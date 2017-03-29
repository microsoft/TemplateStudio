using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;

namespace uct.SplitViewProject.Helper
{
    internal static class ResourceExtensions
    {
        public static string GetLocalized(this string resourceKey)
        {
            try
            {
                var result = new ResourceLoader().GetString(resourceKey);

                if (!string.IsNullOrEmpty(result))
                {
                    return result;
                }
            }
            catch (COMException)
            {
                //IF .RESW FILE IS MISSING WE GET THIS EXCEPTION
            }
            return resourceKey;
        }
    }
}
