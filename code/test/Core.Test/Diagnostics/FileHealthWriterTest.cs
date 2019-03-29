// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

using Microsoft.Templates.Core.Diagnostics;

using Xunit;

namespace Microsoft.Templates.Core.Test.Diagnostics
{
    [Trait("ExecutionSet", "Minimum")]

    public class FileHealthWriterTest
    {
        [Fact]
        public async Task LogInfoAsync()
        {
            string uniqueMsg = $"LogInfo_{Guid.NewGuid()}";

            await FileHealthWriter.Current.WriteTraceAsync(TraceEventType.Information, uniqueMsg);

            AssertMessageIsInLog(FileHealthWriter.Current.LogFileName, uniqueMsg);
        }

        [Fact]
        public async Task LogErrorAsync()
        {
            string uniqueMsg = $"LogError_{Guid.NewGuid()}";

            await FileHealthWriter.Current.WriteTraceAsync(TraceEventType.Error, uniqueMsg);

            AssertMessageIsInLog(FileHealthWriter.Current.LogFileName, uniqueMsg);
        }

        [Fact]
        public async Task LogErrorWithExAsync()
        {
            string uniqueMsg = $"LogErrorWithEx_{Guid.NewGuid()}";

            await FileHealthWriter.Current.WriteTraceAsync(TraceEventType.Error, uniqueMsg, new Exception("SampleException"));

            AssertMessageIsInLog(FileHealthWriter.Current.LogFileName, uniqueMsg);
        }

        [Fact]
        public async Task LogExAsync()
        {
            string uniqueMsg = $"LogException_{Guid.NewGuid()}";

            await FileHealthWriter.Current.WriteExceptionAsync(new Exception("SampleException"), uniqueMsg);

            AssertMessageIsInLog(FileHealthWriter.Current.LogFileName, uniqueMsg);
        }

        [Fact]
        public async Task LogEx_NoExAsync()
        {
            string uniqueMsg = $"LogException_{Guid.NewGuid()}";

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await FileHealthWriter.Current.WriteExceptionAsync(null);
            });
        }

        [Fact]
        public async Task TwoManageThreadsAsync()
        {
            var t1 = Task.Run(async () =>
            {
                string uniqueMsg = $"TwoManageThreads_InstanceA_{Guid.NewGuid()}";

                await FileHealthWriter.Current.WriteTraceAsync(TraceEventType.Error, uniqueMsg, new Exception("SampleException"));

                AssertMessageIsInLog(FileHealthWriter.Current.LogFileName, uniqueMsg);
            });

            var t2 = Task.Run(async () =>
            {
                string otherUniqueMsg = $"TwoManageThreads_InstanceB_{Guid.NewGuid()}";

                await FileHealthWriter.Current.WriteTraceAsync(TraceEventType.Information, otherUniqueMsg);

                AssertMessageIsInLog(FileHealthWriter.Current.LogFileName, otherUniqueMsg);
            });

            await Task.WhenAll(t1, t2);
        }

        private void AssertMessageIsInLog(string logFileName, string uniqueMsg)
        {
            if (File.Exists(logFileName))
            {
                FileStream fs = new FileStream(logFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader sr = new StreamReader(fs);
                string content = sr.ReadToEnd();

                sr.Close();

                Assert.Contains(uniqueMsg, content);
            }
        }
    }
}
