using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core
{
    public class Configuration
    {
        public string CdnUrl { get; set; }
        public string AppInsightsKey { set; get; }

        private static Configuration _current;
        public static Configuration Current
        {
            get
            {
                if (_current == null)
                {
                    _current = new Configuration();
                }
                return _current;
            }    
        }

        internal bool Local
        {
            get
            {
#if (!VSIX_PUBLISH)
                return true;
#else
                return false;
#endif
            }
        }

        private Configuration()
        {
            CdnUrl = Local ? "https://uwpcommunitytemplates.blob.core.windows.net/vnext/Latest" : "###CdnUrl###";
            AppInsightsKey = Local ? "<SET_YOUR_OWN_APP_INSIGHTS_INSTRUMENTATION_KEY>" : "###AppInsightsKey###";
        }
    }
}
