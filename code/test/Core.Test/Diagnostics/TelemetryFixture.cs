using Microsoft.Templates.Core.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.Test.Diagnostics
{
    public class TelemetryFixture : IDisposable
    {
        public TelemetryService Telemetry { get; }
        public TelemetryFixture()
        {
            EnsureCurrentConfigWithTelemetryKey();

            Telemetry = TelemetryService.Current;
        } 

        public static void EnsureCurrentConfigWithTelemetryKey()
        {
            Configuration config = Configuration.Current;
            config.RemoteTelemetryKey = ""; //SECRET
            Configuration.UpdateCurrentConfiguration(config);
        }

        public void Dispose()
        {
            TelemetryService.Current.Dispose();
        }
    }
}
