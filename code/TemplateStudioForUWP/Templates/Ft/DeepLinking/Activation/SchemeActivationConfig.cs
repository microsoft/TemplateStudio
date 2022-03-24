using System;
using System.Collections.Generic;
using System.Linq;

namespace Param_RootNamespace.Activation
{
    public static class SchemeActivationConfig
    {
        private static readonly Dictionary<string, Type> _activationPages = new Dictionary<string, Type>()
        {
            // TODO WTS: Add the pages that can be opened from scheme activation in your app here.
            { "sample", typeof(Views.SchemeActivationSamplePage) }
        };

        public static Type GetPage(string pageKey)
        {
            return _activationPages
                    .FirstOrDefault(p => p.Key == pageKey)
                    .Value;
        }

        public static string GetPageKey(Type pageType)
        {
            return _activationPages
                    .FirstOrDefault(v => v.Value == pageType)
                    .Key;
        }
    }
}
