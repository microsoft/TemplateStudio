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
        public async Task TrackNewProjectAsync()
        {
            await _tracker.TrackNewProjectAsync(ActionStatus.Completed, "appType", "appFx", "templateName");
        }
        [Fact]
        public async Task TrackNewProjectWithMetrics()
        {
            await _tracker.TrackNewProjectAsync(ActionStatus.Completed, "appType", "appFx", "templateName", 10, 10, 1, 2, 2);
        }

        public async Task TrackNewPageAsync()
        {
            await _tracker.TrackNewPageAsync(ActionStatus.Completed, "appType", "appFx", "templateName");
        }
        [Fact]
        public async Task TrackWizardAsync()
        {
            await _tracker.TrackWizardAsync(3);
        }

    }
}
