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
    public class FileDiagnosticListenerTest 
    {
        FileDiagnosticListener _defaultListener = FileDiagnosticListener.Default;
        string _expectedLogFileName;

        public FileDiagnosticListenerTest()
        {
            string dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), @"UWPTemplates\Logs");
            _expectedLogFileName = Path.Combine(dir, $"UWPTemplates_{DateTime.Now.ToString("yyyyMMdd")}.log");
        }

        [Fact]
        public void LogFileProperlyCreated()
        {
            Assert.Equal(_expectedLogFileName, _defaultListener.LogFileName);
            Assert.True(File.Exists(_expectedLogFileName));
        }

        [Fact]
        public void LogInfo()
        {
            string uniqueMsg = $"LogInfo_{Guid.NewGuid()}";
            _defaultListener.WriteDataAsync(TraceLevel.Info, uniqueMsg).Wait();

            AssertMessageIsInLog(_defaultListener.LogFileName, uniqueMsg);
        }

        [Fact]
        public void LogError()
        {
            string uniqueMsg = $"LogError_{Guid.NewGuid()}";
            _defaultListener.WriteDataAsync(TraceLevel.Error, uniqueMsg).Wait();

            AssertMessageIsInLog(_defaultListener.LogFileName, uniqueMsg);
        }

        [Fact]
        public void LogErrorWithEx()
        {
            string uniqueMsg = $"LogErrorWithEx_{Guid.NewGuid()}";
            _defaultListener.WriteDataAsync(TraceLevel.Error, uniqueMsg, new Exception("SampleException")).Wait();

            AssertMessageIsInLog(_defaultListener.LogFileName, uniqueMsg);
        }


        [Fact]
        public void TwoInstances()
        {
            FileDiagnosticListener otherListener = new FileDiagnosticListener();

            string uniqueMsg = $"TwoInstances_InstanceA_{Guid.NewGuid()}";
            _defaultListener.WriteDataAsync(TraceLevel.Error, uniqueMsg, new Exception("SampleException")).Wait();

            string otherUniqueMsg = $"TwoInstances_InstanceB_{Guid.NewGuid()}";
            otherListener.WriteDataAsync(TraceLevel.Info, otherUniqueMsg).Wait();

            AssertMessageIsInLog(_defaultListener.LogFileName, uniqueMsg);
            AssertMessageIsInLog(otherListener.LogFileName, otherUniqueMsg);
        }


        private void AssertMessageIsInLog(string logFileName, string uniqueMsg)
        {
            FileStream fs = new FileStream(logFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Read);
            StreamReader sr = new StreamReader(fs);
            string content = sr.ReadToEnd();
            sr.Close();
            Assert.True(content.Contains(uniqueMsg));
        }
    }
}
