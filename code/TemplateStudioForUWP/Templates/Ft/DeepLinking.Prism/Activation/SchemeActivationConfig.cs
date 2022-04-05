using System;
using System.Collections.Generic;
using System.Linq;

namespace Param_RootNamespace.Activation
{
    public static class SchemeActivationConfig
    {
        private static readonly Dictionary<string, string> _activationPages = new Dictionary<string, string>()
        {
            // TODO: Add the pages that can be opened from scheme activation in your app here.
            { "sample", PageTokens.SchemeActivationSamplePage }
        };

        public static string GetPage(string pageKey)
        {
            return _activationPages
                    .FirstOrDefault(p => p.Key == pageKey)
                    .Value;
        }

        public static string GetPageKey(string pageToken)
        {
            return _activationPages
                    .FirstOrDefault(v => v.Value == pageToken)
                    .Key;
        }
    }
}
