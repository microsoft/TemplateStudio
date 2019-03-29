// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;

using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Resources;

using SystemTasks = System.Threading.Tasks;

namespace Microsoft.Templates.Core.Diagnostics
{
    public class ShellHealthWriter : IHealthWriter
    {
        private GenShell _shell;

        public ShellHealthWriter(GenShell shell)
        {
            _shell = shell;
        }

        public async SystemTasks.Task WriteExceptionAsync(Exception ex, string message = null)
        {
            if (_shell != null)
            {
                await SafeTrackAsync(() =>
                {
                    string header = $"========== {StringRes.ExceptionTrackedString} [{DateTime.Now.FormatAsFullDateTime()}] ==========\n";
                    _shell.WriteOutput(header);

                    if (message != null)
                    {
                        _shell.WriteOutput($"{StringRes.AdditionalMessageString}: {message}\n");
                    }

                    _shell.WriteOutput($"{ex.ToString()}\n");

                    string footer = $"{new string('-', header.Length - 2)}\n";
                    _shell.WriteOutput(footer);
                });
            }
        }

        public async SystemTasks.Task WriteTraceAsync(TraceEventType eventType, string message, Exception ex = null)
        {
            if (_shell != null)
            {
                await SafeTrackAsync(() =>
                {
                    string eventMessage = $"[{DateTime.Now.FormatAsTime()} - {eventType}]::{message}\n";
                    _shell.WriteOutput(eventMessage);

                    if (ex != null)
                    {
                        string header = $"----------- {StringRes.AddtionalExceptionInfoString} -----------\n";
                        string footer = $"{new string('-', header.Length - 2)}\n";
                        string exceptionInfo = header + $"{ex}\n" + footer;
                        _shell.WriteOutput(exceptionInfo);
                    }
                });
            }
        }

        private async SystemTasks.Task SafeTrackAsync(Action trackAction)
        {
            try
            {
                var task = SystemTasks.Task.Run(() =>
                {
                    trackAction();
                });

                await task;
            }
            catch (AggregateException aggEx)
            {
                foreach (Exception ex in aggEx.InnerExceptions)
                {
                    Trace.TraceError("Error writing to Visual Studio Output: {0}", ex.ToString());
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error writing to Visual Studio Output: {0}", ex.ToString());
            }
        }

        public bool AllowMultipleInstances()
        {
            return false;
        }
    }
}
