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
        public virtual string CdnUrl { get; set; } = IsLocalExecution ? "https://uwpcommunitytemplates.blob.core.windows.net/vnext/Latest" : "###CdnUrl###";
        public virtual string RemoteTelemetryKey { get; set; } = IsLocalExecution ? "<SET_YOUR_OWN_KEY>" : "###RemoteTelemetryKey###";
        public virtual string LogFileFolderPath { get; set; } = @"UWPTemplates\Logs";
        public virtual TraceEventType DiagnosticsTraceLevel { get; set; } = IsLocalExecution ? TraceEventType.Verbose : (TraceEventType)Enum.Parse(typeof(TraceEventType), "###DiagnosticsTraceLevel###", true);
        public virtual int DaysToKeepDiagnosticsLogs { get; set; } = 5;
        public int VersionCheckingExpirationMinutes { get; set; } = IsLocalExecution ? 5 : 120;

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

        public Configuration()
        {
        }
        public static void UpdateCurrentConfiguration(Configuration config)
        {
            _current = config;
        }
        protected static bool IsLocalExecution
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
    }
}
