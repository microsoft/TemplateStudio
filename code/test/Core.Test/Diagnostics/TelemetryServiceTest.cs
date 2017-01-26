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
    public class TelemetryServiceTest : IClassFixture<TelemetryFixture>
    {
        TelemetryFixture _fixture;
        public TelemetryServiceTest(TelemetryFixture fixture)
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
        public async Task TrackEventAsync()
        {
            _fixture.Telemetry.Properties.Add(TelemetryEventProperty.Name, "TestTelemetrySampleTemplate");
            _fixture.Telemetry.Properties.Add(TelemetryEventProperty.Framework, "MVVMLight");
            _fixture.Telemetry.Properties.Add(TelemetryEventProperty.Type, "Project");
            await _fixture.Telemetry.TrackEventAsync(TelemetryEvents.TemplateGenerated);

            //Check telemetry properties and metrics are cleared after the event is tracked
            Assert.Equal(0, _fixture.Telemetry.Properties.Count);
            Assert.Equal(0, _fixture.Telemetry.Metrics.Count);
        }

        [Fact]
        public async Task TrackTwoEventsAsync()
        {
            _fixture.Telemetry.Properties.Add(TelemetryEventProperty.Name, "TestTelemetrySampleTemplate");
            _fixture.Telemetry.Properties.Add(TelemetryEventProperty.Framework, "MVVMLight");
            _fixture.Telemetry.Properties.Add(TelemetryEventProperty.Type, "Project");
            await _fixture.Telemetry.TrackEventAsync(TelemetryEvents.TemplateGenerated);

            //Check telemetry properties and metrics are cleared after the event is tracked
            Assert.Equal(0, _fixture.Telemetry.Properties.Count);
            Assert.Equal(0, _fixture.Telemetry.Metrics.Count);

            _fixture.Telemetry.Properties.Add(TelemetryEventProperty.Name, "OtherData");
            _fixture.Telemetry.Properties.Add(TelemetryEventProperty.Framework, "Caliburn");
            _fixture.Telemetry.Properties.Add(TelemetryEventProperty.Type, "Page");
            await _fixture.Telemetry.TrackEventAsync(TelemetryEvents.TemplateGenerated);

            //Check telemetry properties and metrics are cleared after the event is tracked
            Assert.Equal(0, _fixture.Telemetry.Properties.Count);
            Assert.Equal(0, _fixture.Telemetry.Metrics.Count);
        }

        [Fact]
        public async Task TrackExceptionAsync()
        {
            await _fixture.Telemetry.TrackExceptionAsync(new Exception("Telemetry Test TrackException"));

            //Check telemetry properties and metrics are cleared after the event is tracked
            Assert.Equal(0, _fixture.Telemetry.Properties.Count);
            Assert.Equal(0, _fixture.Telemetry.Metrics.Count);
        }

    }
}
