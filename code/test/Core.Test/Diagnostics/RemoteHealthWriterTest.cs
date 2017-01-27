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
        [Fact]
        public async Task WriteExceptionAsync()
        {
            await RemoteHealthWriter.Current.WriteExceptionAsync(new Exception("New Test Exception from RemoteDiagnosticsWriterTest"));
        }

        [Fact]
        public async Task WriteEventAsync()
        {
            await RemoteHealthWriter.Current.WriteExceptionAsync(new Exception("New Test Exception from RemoteDiagnosticsWriterTest"));
        }
    }
}
