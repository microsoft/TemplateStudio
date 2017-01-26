using Microsoft.Templates.Core.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Microsoft.Templates.Core.Test.Diagnostics
{
    public class TelemetryTrackerTest 
    {
        TelemetryTracker _tracker;
        public TelemetryTrackerTest()
        {
            TelemetryFixture.EnsureCurrentConfigWithTelemetryKey();
            _tracker = new TelemetryTracker(Configuration.Current);
        }
        [Fact]
        public async Task TrackEventAsync()
        {
            await _tracker.TrackTemplateGeneratedAsync("TemplateName", "TemplateFramework", "TemplateType");
        }
    }
}
