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
        public void LogInfo()
        {
            string uniqueMsg = $"LogInfo_{Guid.NewGuid()}";
            _fixture.FileLogWriter.WriteTraceAsync(TraceEventType.Information, uniqueMsg).Wait();

            AssertMessageIsInLog(_fixture.FileLogWriter.LogFileName, uniqueMsg);
        }

        [Fact]
        public void LogError()
        {
            string uniqueMsg = $"LogError_{Guid.NewGuid()}";
            _fixture.FileLogWriter.WriteTraceAsync(TraceEventType.Error, uniqueMsg).Wait();

            AssertMessageIsInLog(_fixture.FileLogWriter.LogFileName, uniqueMsg);
        }

        [Fact]
        public void LogErrorWithEx()
        {
            string uniqueMsg = $"LogErrorWithEx_{Guid.NewGuid()}";
            _fixture.FileLogWriter.WriteTraceAsync(TraceEventType.Error, uniqueMsg, new Exception("SampleException")).Wait();

            AssertMessageIsInLog(_fixture.FileLogWriter.LogFileName, uniqueMsg);
        }


        [Fact]
        public void TwoInstances()
        {
            FileHealthWriter otherListener = new FileHealthWriter(Configuration.Current);

            string uniqueMsg = $"TwoInstances_InstanceA_{Guid.NewGuid()}";
            _fixture.FileLogWriter.WriteTraceAsync(TraceEventType.Error, uniqueMsg, new Exception("SampleException")).Wait();

            string otherUniqueMsg = $"TwoInstances_InstanceB_{Guid.NewGuid()}";
            otherListener.WriteTraceAsync(TraceEventType.Information, otherUniqueMsg).Wait();

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
