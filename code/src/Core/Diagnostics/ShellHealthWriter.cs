using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemTasks = System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio;
using System.Reflection;

namespace Microsoft.Templates.Core.Diagnostics
{
    public class ShellHealthWriter : IHealthWriter
    {
        private GenShell _shell; 
         
        public ShellHealthWriter(GenShell shell)
        {
            _shell = shell ?? throw new ArgumentNullException("shell");
        }

        public async SystemTasks.Task WriteExceptionAsync(Exception ex, string message = null)
        {
            if (_shell != null)
            {
                await SafeTrackAsync(() =>
                {
                    string header = $"========== Tracked Exception [{DateTime.Now.ToString("yyyyMMdd hh:mm:ss.fff")}] ==========\n";
                    _shell.WriteOutput(header);

                    if (message != null)
                    {
                        _shell.WriteOutput($"AdditionalMessage: {message}\n");
                    }

                    _shell.WriteOutput($"{ex.ToString()}\n");

                    string footer = $"{new String('-', header.Length - 2)}\n";
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
                    string eventMessage = $"[{DateTime.Now.ToString("hh:mm:ss.fff")} - {eventType.ToString()}]::{message}\n";
                    _shell.WriteOutput(eventMessage);
                    if (ex != null)
                    {
                        string header = $"----------- Addtional Exception Info -----------\n";
                        string footer = $"{new String('-', header.Length - 2)}\n";
                        string exceptionInfo = header + $"{ex.ToString()}\n" + footer;
                        _shell.WriteOutput(exceptionInfo);
                    }
                });
            }
        }

       

        private async SystemTasks.Task SafeTrackAsync(Action trackAction)
        {
            try
            {
                var task = SystemTasks.Task.Run(() => {
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
