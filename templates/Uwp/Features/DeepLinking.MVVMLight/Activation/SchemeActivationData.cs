using System;
using System.Collections.Generic;
using System.Web;

namespace Param_ItemNamespace.Activation
{
    public class SchemeActivationData
    {
        private const string ProtocolName = "wtsapp";

        public string ViewModelName { get; private set; }

        public Uri Uri { get; private set; }

        public Dictionary<string, string> Parameters { get; private set; } = new Dictionary<string, string>();

        public bool IsValid => ViewModelName != null;

        public SchemeActivationData(Uri activationUri)
        {
            ViewModelName = SchemeActivationConfig.GetViewModelName(activationUri.AbsolutePath);

            if (!IsValid || string.IsNullOrEmpty(activationUri.Query))
            {
                return;
            }

            var uriQuery = HttpUtility.ParseQueryString(activationUri.Query);
            foreach (var paramKey in uriQuery.AllKeys)
            {
                Parameters.Add(paramKey, uriQuery.Get(paramKey));
            }
        }

        public SchemeActivationData(string viewModelName, Dictionary<string, string> parameters = null)
        {
            ViewModelName = viewModelName;
            Parameters = parameters;
            Uri = BuildUri();
        }

        private Uri BuildUri()
        {
            var viewModelKey = SchemeActivationConfig.GetViewModelKey(ViewModelName);
            var uriBuilder = new UriBuilder($"{ProtocolName}:{viewModelKey}");
            var query = HttpUtility.ParseQueryString(string.Empty);

            foreach (var parameter in Parameters)
            {
                query.Set(parameter.Key, parameter.Value);
            }

            uriBuilder.Query = query.ToString();
            return new Uri(uriBuilder.ToString());
        }
    }
}