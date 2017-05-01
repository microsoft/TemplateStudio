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
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.Templates.Core.Diagnostics;

namespace Microsoft.Templates.Core.Test.Diagnostics
{
    public class TestHealthWriter : IHealthWriter
    {
        public List<string> Events = new List<string>();
        public List<Exception> Exceptions = new List<Exception>();

        public async Task WriteTraceAsync(TraceEventType eventType, string message, Exception ex = null)
        {
            await Task.Run(() => Events.Add($"{eventType.ToString()};{message};{ex?.ToString()}"));
        }

        public async Task WriteExceptionAsync(Exception ex, string message = null)
        {
            await Task.Run(() => Exceptions.Add(ex));
        }

        public bool AllowMultipleInstances()
        {
            return true;
        }
    }
}
