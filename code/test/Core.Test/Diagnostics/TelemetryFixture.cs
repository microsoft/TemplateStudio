using Microsoft.Templates.Core.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.Test.Diagnostics
{
    public class TelemetryFixture
    {
        public TelemetryService Telemetry { get; }
        public TelemetryFixture()
        {
            Telemetry = new TelemetryService(new TestConfiguration());
        } 
    }
}
