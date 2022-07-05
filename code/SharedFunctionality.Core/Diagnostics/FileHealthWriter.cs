// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Templates.SharedResources;

namespace Microsoft.Templates.Core.Diagnostics
{
    public class FileHealthWriter : IHealthWriter
    {
        private static SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);

        private string _workingFolder;

        public string LogFileName { get; private set; }

        private static FileHealthWriter _current;

        public static FileHealthWriter Current
        {
            get
            {
                if (_current == null)
                {
                    _current = new FileHealthWriter();
                }

                return _current;
            }
        }

        private FileHealthWriter()
        {
            _workingFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData, Environment.SpecialFolderOption.Create), Configuration.Current.LogFileFolderPath);

            InitializeLogFile();
            PurgeOldLogs(_workingFolder, Configuration.Current.DaysToKeepDiagnosticsLogs);
        }

        public async Task WriteTraceAsync(TraceEventType eventType, string message, Exception ex = null)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"{FormattedWriterMessages.LogEntryStart}\t{eventType.ToString()}\t{message}");

            if (ex != null)
            {
                sb.AppendLine(FormattedWriterMessages.ExHeader);
                sb.AppendLine(ex.ToString());
                sb.AppendLine(FormattedWriterMessages.ExFooter);
            }

            await WriteAndFlushAsync(sb.ToString());
        }

        public async Task WriteExceptionAsync(Exception ex, string message = null)
        {
            if (ex == null)
            {
                throw new ArgumentNullException(nameof(ex));
            }

            var sb = new StringBuilder();
            sb.AppendLine($"{FormattedWriterMessages.LogEntryStart}\t{TraceEventType.Critical.ToString():11}\t{Resources.ExceptionTrackedString}. {message ?? string.Empty}");

            if (ex != null)
            {
                sb.AppendLine(FormattedWriterMessages.ExHeader);
                sb.AppendLine(ex.ToString());
                sb.AppendLine(FormattedWriterMessages.ExFooter);
            }

            await WriteAndFlushAsync(sb.ToString());
        }

        public bool AllowMultipleInstances()
        {
            return false;
        }

        private async Task WriteAndFlushAsync(string data)
        {
            await _semaphoreSlim.WaitAsync();

            try
            {
                File.AppendAllText(LogFileName, data);
            }
            catch (Exception exception)
            {
                Trace.TraceError($"Error writing to the stream. Exception:\r\n{exception.ToString()}");
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        private void InitializeLogFile()
        {
            _semaphoreSlim.Wait();

            try
            {
                LogFileName = Path.Combine(_workingFolder, $"WTS_{Configuration.Current.Environment}_{DateTime.Now.FormatAsDateForFilePath()}.log");

                if (!Directory.Exists(_workingFolder))
                {
                    Directory.CreateDirectory(_workingFolder);
                }

                if (CheckLogFileInUse(LogFileName))
                {
                    LogFileName = LogFileName.Replace(".log", $"_{Guid.NewGuid().ToString()}.log");
                }

                StartLog();
            }
            catch (Exception exception)
            {
                Trace.TraceError($"Error initializating log file. Exception:\r\n{exception.ToString()}");
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        private void StartLog()
        {
            var stackTrace = new StackTrace();
            var stackFrames = stackTrace.GetFrames();

            var sb = new StringBuilder();

            sb.AppendLine($"\r\n>>>>>>>>>>>>>> {Resources.LogStartedString} {DateTime.Now.FormatAsFullDateTime()}");
            sb.AppendLine($">>>>>>>>>>>>>> {Resources.AssemblyFileVersionString}: {GetVersion()}");

            File.AppendAllText(LogFileName, sb.ToString());
        }

        private static string GetVersion()
        {
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;

            return FileVersionInfo.GetVersionInfo(assemblyLocation).FileVersion;
        }

        private static bool CheckLogFileInUse(string logFileName)
        {
            FileStream stream = null;
            try
            {
                stream = new FileStream(logFileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }

            return false;
        }

        private static void PurgeOldLogs(string logFolder, int daysToKeep)
        {
            if (Directory.Exists(logFolder))
            {
                var di = new DirectoryInfo(logFolder);
                var toBeDeleted = di.GetFiles().Where(f => f.CreationTimeUtc.AddDays(daysToKeep) < DateTime.UtcNow);

                foreach (var f in toBeDeleted)
                {
                    try
                    {
                        f.Delete();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error removing old log file '{f.FullName}'. Skipped. Exception:\n\r{ex.ToString()}");
                        Trace.TraceError($"Error removing old log file '{f.FullName}'. Skipped. Exception:\n\r{ex.ToString()}");
                    }
                }
            }
        }
    }
}
