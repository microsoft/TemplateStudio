using Microsoft.Templates.Core.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core
{
    public class Configuration
    {
        public string CdnUrl { get; set; } = IsLocalExecution ? "https://uwpcommunitytemplates.blob.core.windows.net/vnext/Latest" : "###CdnUrl###";
        public string RemoteTelemetryKey { get; set; } = IsLocalExecution ? "<SET_YOUR_OWN_KEY>" : "###RemoteTelemetryKey###";
        public string LogFileFolderPath { get; set; } = @"UWPTemplates\Logs";
        public TraceEventType DiagnosticsTraceLevel { get; set; } = IsLocalExecution ? TraceEventType.Verbose : (TraceEventType)Enum.Parse(typeof(TraceEventType), "###DiagnosticsTraceLevel###", true);
        public int DaysToKeepDiagnosticsLogs { get; set; } = 5;

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
        public static void UpdateCurrentConfiguration(Configuration config)
        {
            _current = config;
        }

        public Configuration()
        {
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
