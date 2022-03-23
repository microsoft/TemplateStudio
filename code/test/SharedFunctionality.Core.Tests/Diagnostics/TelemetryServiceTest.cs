// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.Templates.Core.Diagnostics;
using Xunit;

namespace Microsoft.Templates.Core.Test.Diagnostics
{
    [Trait("Group", "Minimum")]

    public class TelemetryServiceTest : IClassFixture<TelemetryFixture>
    {
        private TelemetryFixture _fixture;

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
                { TelemetryProperties.Framework, "MVVMLight" },
                { TelemetryProperties.ProjectType, "Blank" },
            };

            await _fixture.Telemetry.TrackEventAsync(TelemetryEvents.ProjectGen, props);
        }

        [Fact]
        public async Task TrackTwoEventsAsync()
        {
            Dictionary<string, string> props = new Dictionary<string, string>
            {
                { TelemetryProperties.TemplateName, "TestTelemetrySampleTemplate" },
                { TelemetryProperties.Framework, "MVVMLight" },
                { TelemetryProperties.ProjectType, "Blank" },
            };

            await _fixture.Telemetry.TrackEventAsync(TelemetryEvents.ProjectGen, props);

            props[TelemetryProperties.TemplateName] = "OtherData";
            props[TelemetryProperties.Framework] = "Caliburn";
            props[TelemetryProperties.ProjectType] = "SplitView";

            await _fixture.Telemetry.TrackEventAsync(TelemetryEvents.ProjectGen, props);
        }

        [Fact]
        public async Task TrackExceptionAsync()
        {
            await _fixture.Telemetry.TrackExceptionAsync(new Exception("Telemetry Test TrackException"));
        }
    }
}
