using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Microsoft.Templates.UI.Services
{
    public static class ResourceService
    {
        private static Window _mainView;

        public static void Initialize(Window mainWindow)
        {
            _mainView = mainWindow;
        }

        public static T FindResource<T>(string resourceKey) where T : class
        {
            if (_mainView != null)
            {
                return _mainView.FindResource(resourceKey) as T;
            }
            return default(T);
        }
    }
}
