using Microsoft.Templates.Core.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.Test.Diagnostics
{
    public sealed class TelemetryFixture : IDisposable
    {
        public TelemetryService Telemetry { get; }
        public TelemetryFixture()
        {
            Telemetry = TelemetryService.Current;
        } 

        public void Dispose()
        {
            TelemetryService.Current.Dispose();
        }
    }
}
