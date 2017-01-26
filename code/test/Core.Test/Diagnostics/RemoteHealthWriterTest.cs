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
        RemoteHealthWriter _writer = new RemoteHealthWriter(new TestConfiguration());
        [Fact]
        public async Task WriteExceptionAsync()
        {
            await _writer.WriteExceptionAsync(new Exception("New Test Exception from RemoteDiagnosticsWriterTest"));
        }
    }
}
