using System;
using System.Diagnostics;
using SystemTasks = System.Threading.Tasks;

namespace Microsoft.Templates.Core.Diagnostics
{
    public class ShellHealthWriter : IHealthWriter
    {       
        public async SystemTasks.Task WriteExceptionAsync(Exception ex, string message = null)
        {
            if (GenShell.Current != null)
            {
                await SafeTrackAsync(() =>
                {
                    string header = $"========== Tracked Exception [{DateTime.Now.ToString("yyyyMMdd hh:mm:ss.fff")}] ==========\n";
                    GenShell.Current.WriteOutput(header);

                    if (message != null)
                    {
                        GenShell.Current.WriteOutput($"AdditionalMessage: {message}\n");
                    }

                    GenShell.Current.WriteOutput($"{ex.ToString()}\n");

                    string footer = $"{new String('-', header.Length - 2)}\n";
                    GenShell.Current.WriteOutput(footer);
                });
            }
        }

        public async SystemTasks.Task WriteTraceAsync(TraceEventType eventType, string message, Exception ex = null)
        {
            if (GenShell.Current != null)
            {
                await SafeTrackAsync(() =>
                {
                    string eventMessage = $"[{DateTime.Now.ToString("hh:mm:ss.fff")} - {eventType.ToString()}]::{message}\n";
                    GenShell.Current.WriteOutput(eventMessage);
                    if (ex != null)
                    {
                        string header = $"----------- Addtional Exception Info -----------\n";
                        string footer = $"{new String('-', header.Length - 2)}\n";
                        string exceptionInfo = header + $"{ex.ToString()}\n" + footer;
                        GenShell.Current.WriteOutput(exceptionInfo);
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
