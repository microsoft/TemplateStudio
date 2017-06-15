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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
            _workingFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), Configuration.Current.LogFileFolderPath);

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
                throw new ArgumentNullException("ex");
            }

            var sb = new StringBuilder();
            sb.AppendLine($"{FormattedWriterMessages.LogEntryStart}\t{TraceEventType.Critical.ToString():11}\tException Tracked. {(message ?? "")}");

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
                LogFileName = Path.Combine(_workingFolder, $"WTS_{Configuration.Current.Environment}_{DateTime.Now.ToString("yyyyMMdd")}.log");

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

            sb.AppendLine($"\r\n>>>>>>>>>>>>>> Log started {DateTime.Now.ToString("yyyyMMdd hh:mm:ss.fff")}");
            sb.AppendLine($">>>>>>>>>>>>>> Assembly File Version: {GetVersion()}");

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
