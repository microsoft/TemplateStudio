using Microsoft.Templates.Core.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.Test.Diagnostics
{
    public class TestConfiguration : Configuration
    {
        public new string RemoteTelemetryKey
        {
            get
            {
                return ""; //TODO: Remove in GH
            }
        }
    }
    public class TelemetryFixture
    {
        public Telemetry Telemetry { get; }
        public TelemetryFixture()
        {
                Telemetry = new Telemetry(new TestConfiguration());
        } 
    }
}
