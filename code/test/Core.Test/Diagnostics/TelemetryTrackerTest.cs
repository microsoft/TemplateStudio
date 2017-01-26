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
        TelemetryTracker Tracker = new TelemetryTracker(new TestConfiguration());
        [Fact]
        public async Task TrackEventAsync()
        {
            await Tracker.TrackTemplateGeneratedAsync("TemplateName", "TemplateFramework", "TemplateType");
        }
    }
}
