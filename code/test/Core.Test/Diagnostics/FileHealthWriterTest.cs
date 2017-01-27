using Microsoft.Templates.Core.Diagnostics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Microsoft.Templates.Core.Test.Diagnostics
{
    public class FileHealthWriterTest : IClassFixture<FileHealthFixture>
    {
        FileHealthFixture _fixture;
        public FileHealthWriterTest(FileHealthFixture fixture)
        {
            _fixture = fixture;
        }        
        [Fact]
        public async Task LogInfo()
        {
            string uniqueMsg = $"LogInfo_{Guid.NewGuid()}";
            await _fixture.FileLogWriter.WriteTraceAsync(TraceEventType.Information, uniqueMsg);

            AssertMessageIsInLog(_fixture.FileLogWriter.LogFileName, uniqueMsg);
        }

        [Fact]
        public async Task LogError()
        {
            string uniqueMsg = $"LogError_{Guid.NewGuid()}";
            await _fixture.FileLogWriter.WriteTraceAsync(TraceEventType.Error, uniqueMsg);

            AssertMessageIsInLog(_fixture.FileLogWriter.LogFileName, uniqueMsg);
        }

        [Fact]
        public async Task LogErrorWithEx()
        {
            string uniqueMsg = $"LogErrorWithEx_{Guid.NewGuid()}";
            await _fixture.FileLogWriter.WriteTraceAsync(TraceEventType.Error, uniqueMsg, new Exception("SampleException"));

            AssertMessageIsInLog(_fixture.FileLogWriter.LogFileName, uniqueMsg);
        }


        [Fact]
        public async Task TwoInstances()
        {
            FileHealthWriter otherListener = new FileHealthWriter(Configuration.Current);

            string uniqueMsg = $"TwoInstances_InstanceA_{Guid.NewGuid()}";
            await _fixture.FileLogWriter.WriteTraceAsync(TraceEventType.Error, uniqueMsg, new Exception("SampleException"));

            string otherUniqueMsg = $"TwoInstances_InstanceB_{Guid.NewGuid()}";
            await otherListener.WriteTraceAsync(TraceEventType.Information, otherUniqueMsg);

            AssertMessageIsInLog(_fixture.FileLogWriter.LogFileName, uniqueMsg);
            AssertMessageIsInLog(otherListener.LogFileName, otherUniqueMsg);
        }


        private void AssertMessageIsInLog(string logFileName, string uniqueMsg)
        {
            FileStream fs = new FileStream(logFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader sr = new StreamReader(fs);
            string content = sr.ReadToEnd();
            sr.Close();
            Assert.True(content.Contains(uniqueMsg));
        }
    }
}
