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
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Debug.WriteLine($"Unhandledcito: \n\r{e.ExceptionObject.ToString()}");
        }

        [Fact]
        public void Instantiated()
        {
            Assert.NotNull(_fixture.Telemetry);
        }

        [Fact]
        public async Task TrackEventAsync()
        {
            Dictionary<string, string> props = new Dictionary<string, string>();
            props.Add(TelemetryEventProperty.Name, "TestTelemetrySampleTemplate");
            props.Add(TelemetryEventProperty.Framework, "MVVMLight");
            props.Add(TelemetryEventProperty.Type, "Project");
            await _fixture.Telemetry.TrackEventAsync(TelemetryEvents.TemplateGenerated);
        }

        [Fact]
        public async Task TrackTwoEventsAsync()
        {
            Dictionary<string, string> props = new Dictionary<string, string>();
            props.Add(TelemetryEventProperty.Name, "TestTelemetrySampleTemplate");
            props.Add(TelemetryEventProperty.Framework, "MVVMLight");
            props.Add(TelemetryEventProperty.Type, "Project");
            await _fixture.Telemetry.TrackEventAsync(TelemetryEvents.TemplateGenerated, props);

            props[TelemetryEventProperty.Name] = "OtherData";
            props[TelemetryEventProperty.Framework] = "Caliburn";
            props[TelemetryEventProperty.Type] = "Page";
            await _fixture.Telemetry.TrackEventAsync(TelemetryEvents.TemplateGenerated, props);

        }

        [Fact]
        public async Task TrackExceptionAsync()
        {
            await _fixture.Telemetry.TrackExceptionAsync(new Exception("Telemetry Test TrackException"));
        }

    }
}
