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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Templates.Core.Diagnostics;

using Xunit;

namespace Microsoft.Templates.Core.Test.Diagnostics
{
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
                Assert.True(content.Contains(uniqueMsg));
            }
        }
    }
}
