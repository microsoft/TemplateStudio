using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

using System.Diagnostics;
using Microsoft.Templates.Core.Diagnostics;

namespace Microsoft.Templates.Core.Test.Diagnostics
{
    public class TelemetryTest : IClassFixture<TelemetryFixture>
    {
        TelemetryFixture _fixture;
        public TelemetryTest(TelemetryFixture fixture)
        {
            _fixture = fixture;
        }
        [Fact]
        public void Instantiated()
        {
            Assert.NotNull(_fixture.Telemetry);
            Assert.NotNull(_fixture.Telemetry.Properties);
            Assert.NotNull(_fixture.Telemetry.Metrics);
        }

        [Fact]
        public void TrackEvent()
        {
            if (_fixture.Telemetry.IsEnabled)
            {
                _fixture.Telemetry.Properties.Add(TelemetryEventProperty.Name, "TestTelemetrySampleTemplate");
                _fixture.Telemetry.Properties.Add(TelemetryEventProperty.Framework, "MVVMLight");
                _fixture.Telemetry.Properties.Add(TelemetryEventProperty.Type, "Project");
                _fixture.Telemetry.TrackEvent(TelemetryEvents.TemplateGenerated);
            }
        }

        [Fact]
        public void VerifySession()
        {
        }


        [Fact]
        public void CheckIsEnabled()
        {
        }


    }
}
