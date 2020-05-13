using System;
using System.Collections.Generic;
using System.Web;

namespace Param_RootNamespace.Activation
{
    public class SchemeActivationData
    {
        // TODO WTS: Open package.appxmanifest and change the declaration for the scheme (from the default of 'wtsapp') to what you want for your app.
        // More details about this functionality can be found at https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/UWP/features/deep-linking.md
        // TODO WTS: Change the image in Assets/Logo.png to one for display if the OS asks the user which app to launch.
        // Also update this protocol name with the same value as package.appxmanifest.
        private const string ProtocolName = "wtsapp";

        public string PageToken { get; private set; }

        public Uri Uri { get; private set; }

        public Dictionary<string, string> Parameters { get; private set; } = new Dictionary<string, string>();

        public bool IsValid => !string.IsNullOrEmpty(PageToken);

        public SchemeActivationData(Uri activationUri)
        {
            PageToken = SchemeActivationConfig.GetPage(activationUri.AbsolutePath);

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

        public SchemeActivationData(string pageToken, Dictionary<string, string> parameters = null)
        {
            PageToken = pageToken;
            Parameters = parameters;
            Uri = BuildUri();
        }

        private Uri BuildUri()
        {
            var pageKey = SchemeActivationConfig.GetPageKey(PageToken);
            var uriBuilder = new UriBuilder($"{ProtocolName}:{pageKey}");
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
