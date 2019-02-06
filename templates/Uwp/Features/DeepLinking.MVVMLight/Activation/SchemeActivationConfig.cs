using System;
using System.Collections.Generic;
using System.Linq;

namespace Param_RootNamespace.Activation
{
    public static class SchemeActivationConfig
    {
        private static readonly Dictionary<string, string> _activationViewModels = new Dictionary<string, string>()
        {
            // TODO WTS: Add the pages that can be opened from scheme activation in your app here.
            { "sample", typeof(ViewModels.SchemeActivationSampleViewModel).FullName }
        };

        public static string GetViewModelName(string viewModelKey)
        {
            return _activationViewModels
                    .FirstOrDefault(p => p.Key == viewModelKey)
                    .Value;
        }

        public static string GetViewModelKey(string viewModelName)
        {
            return _activationViewModels
                    .FirstOrDefault(v => v.Value == viewModelName)
                    .Key;
        }
    }
}