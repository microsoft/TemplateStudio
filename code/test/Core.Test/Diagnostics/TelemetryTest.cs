using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

using System.Diagnostics;

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
        public void Instantiate()
        {

        }

        [Fact]
        public void VerifyDictionaryAndMetricsInstantiate()
        {
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
