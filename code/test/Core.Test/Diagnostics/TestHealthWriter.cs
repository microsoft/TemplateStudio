// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

using Microsoft.Templates.Core.Diagnostics;

namespace Microsoft.Templates.Core.Test.Diagnostics
{
    public class TestHealthWriter : IHealthWriter
    {
        public List<string> Events { get; set; } = new List<string>();

        public List<Exception> Exceptions { get; set; } = new List<Exception>();

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
