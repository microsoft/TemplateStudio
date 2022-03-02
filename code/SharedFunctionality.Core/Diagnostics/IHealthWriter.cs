// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.Diagnostics
{
    public interface IHealthWriter
    {
        Task WriteTraceAsync(TraceEventType eventType, string message, Exception ex = null);

        Task WriteExceptionAsync(Exception ex, string message = null);

        bool AllowMultipleInstances();
    }
}
