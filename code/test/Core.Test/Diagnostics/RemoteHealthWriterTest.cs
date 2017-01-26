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
    public class RemoteHealthWriterTest
    {
        RemoteHealthWriter _writer;

        public RemoteHealthWriterTest()
        {
            TelemetryFixture.EnsureCurrentConfigWithTelemetryKey();
            _writer = new RemoteHealthWriter(Configuration.Current);
        }

        [Fact]
        public async Task WriteExceptionAsync()
        {
            await _writer.WriteExceptionAsync(new Exception("New Test Exception from RemoteDiagnosticsWriterTest"));
        }
    }
}
