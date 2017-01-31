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
            config.RemoteTelemetryKey = "5e56cd68-9738-469b-baf0-1b6b7c296745"; //SECRET
            Configuration.UpdateCurrentConfiguration(config);
        }

        public void Dispose()
        {
            TelemetryService.Current.Dispose();
        }
    }
}
