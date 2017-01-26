using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core
{
    public class Configuration
    {
        public virtual string CdnUrl { get; set; }
        public virtual string RemoteTelemetryKey { get; set; }
        public virtual string LogFileFolderPath { get; set; }
        public virtual TraceEventType DiagnosticsTraceLevel { get; set; }

        private static Configuration _default;
        public static Configuration Default
        {
            get
            {
                if (_default == null)
                {
                    _default = new Configuration();
                }
                return _default;
            }    
        }

        protected bool Local
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

        protected Configuration()
        {
            CdnUrl = Local ? "https://uwpcommunitytemplates.blob.core.windows.net/vnext/Latest" : "###CdnUrl###";
            RemoteTelemetryKey = Local ? "<SET_YOUR_OWN_KEY>" : "###RemoteTelemetryKey###";
            DiagnosticsTraceLevel = Local ? TraceEventType.Verbose : (TraceEventType)Enum.Parse(typeof(TraceEventType), "###DiagnosticsTraceLevel###", true);
            LogFileFolderPath = @"UWPTemplates\Logs";
        }
    }
}
