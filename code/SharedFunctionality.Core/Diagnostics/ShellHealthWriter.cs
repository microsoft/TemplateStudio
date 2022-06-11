// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;

using Microsoft.Templates.Core.Gen.Shell;
using Microsoft.Templates.SharedResources;

using SystemTasks = System.Threading.Tasks;

namespace Microsoft.Templates.Core.Diagnostics
{
    public class ShellHealthWriter : IHealthWriter
    {
        private readonly IGenShell _shell;

        public ShellHealthWriter(IGenShell shell)
        {
            _shell = shell;
        }

        public async SystemTasks.Task WriteExceptionAsync(Exception ex, string message = null)
        {
            if (_shell != null)
            {
                await SafeTrackAsync(() =>
                {
                    string header = $"========== {Resources.ExceptionTrackedString} [{DateTime.Now.FormatAsFullDateTime()}] ==========\n";
                    _shell.UI.WriteOutput(header);

                    if (message != null)
                    {
                        _shell.UI.WriteOutput($"{Resources.AdditionalMessageString}: {message}\n");
                    }

                    _shell.UI.WriteOutput($"{ex.ToString()}\n");

                    string footer = $"{new string('-', header.Length - 2)}\n";
                    _shell.UI.WriteOutput(footer);
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
                    _shell.UI.WriteOutput(eventMessage);

                    if (ex != null)
                    {
                        string header = $"----------- {Resources.AddtionalExceptionInfoString} -----------\n";
                        string footer = $"{new string('-', header.Length - 2)}\n";
                        string exceptionInfo = header + $"{ex}\n" + footer;
                        _shell.UI.WriteOutput(exceptionInfo);
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
