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
        }

        [Fact]
        public async Task TrackEventAsync()
        {
            Dictionary<string, string> props = new Dictionary<string, string>
            {
                { TelemetryProperties.TemplateName, "TestTelemetrySampleTemplate" },
                { TelemetryProperties.AppFx, "MVVMLight" },
                { TelemetryProperties.AppType, "Blank" }
            };
            await _fixture.Telemetry.TrackEventAsync(TelemetryEvents.Project, props);
        }

        [Fact]
        public async Task TrackTwoEventsAsync()
        {
            Dictionary<string, string> props = new Dictionary<string, string>
            {
                { TelemetryProperties.TemplateName, "TestTelemetrySampleTemplate" },
                { TelemetryProperties.AppFx, "MVVMLight" },
                { TelemetryProperties.AppType, "Blank" }
            };
            await _fixture.Telemetry.TrackEventAsync(TelemetryEvents.Project, props);

            props[TelemetryProperties.TemplateName] = "OtherData";
            props[TelemetryProperties.AppFx] = "Caliburn";
            props[TelemetryProperties.AppType] = "SplitView";
            await _fixture.Telemetry.TrackEventAsync(TelemetryEvents.Project, props);

        }

        [Fact]
        public async Task TrackExceptionAsync()
        {
            await _fixture.Telemetry.TrackExceptionAsync(new Exception("Telemetry Test TrackException"));
        }

    }
}
