// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Templates.Core.Diagnostics;

using Xunit;

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
                { TelemetryProperties.Framework, "MVVMLight" },
                { TelemetryProperties.ProjectType, "Blank" }
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
                { TelemetryProperties.ProjectType, "Blank" }
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
