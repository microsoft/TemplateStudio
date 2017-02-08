using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.Diagnostics
{
    public class FileHealthWriter: IHealthWriter, IDisposable
    {
        private static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);
        private string _workingFolder;

        private StreamWriter _fileStream;

        private string _fileName;
        public string LogFileName
        {
            get
            {
                return _fileName;
            }
        }

        private static FileHealthWriter _current;
        public static FileHealthWriter Current
        {
            get
            {
                if(_current == null)
                {
                    _current = new FileHealthWriter(Configuration.Current);
                }
                return _current;
            }
        }
        private FileHealthWriter(Configuration currentConfig)
        {        
            _workingFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), currentConfig.LogFileFolderPath);

            PurgeOldLogs(_workingFolder, Configuration.Current.DaysToKeepDiagnosticsLogs);
        }

        public static void SetConfiguration(Configuration config)
        {
            _current = new FileHealthWriter(config);
        }

        public async Task WriteTraceAsync(TraceEventType eventType, string message, Exception ex=null)
        {
            await InitializeLogFileAsync();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{FormattedWriterMessages.LogEntryStart}\t{eventType.ToString()}\t{message}");
            if(ex != null)
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

            await InitializeLogFileAsync();

            StringBuilder sb = new StringBuilder();
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

        private async Task InitializeLogFileAsync()
        {
            await semaphoreSlim.WaitAsync();
            try
            {
                if (_fileStream == null)
                {
                    _fileName = Path.Combine(_workingFolder, $"UWPTemplates_{DateTime.Now.ToString("yyyyMMdd")}.log");
                    if (!Directory.Exists(_workingFolder))
                    {
                        Directory.CreateDirectory(_workingFolder);
                    }
                    if (CheckLogFileInUse())
                    {
                        _fileName = _fileName.Replace(".log", $"_{Guid.NewGuid().ToString()}.log");
                    }
                    _fileStream = OpenSharedFileStream(_fileName);

                    StartLog();
                }
            }
            catch (Exception exception)
            {
                Trace.TraceError($"Error initializating log file. Exception:\r\n{exception.ToString()}");
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }

        private StreamWriter OpenSharedFileStream(string fileName)
        {
            FileMode mode = File.Exists(fileName) ? FileMode.Append : FileMode.CreateNew;
            FileStream fs = new FileStream(fileName, mode, FileAccess.Write, FileShare.ReadWrite | FileShare.Read);
            StreamWriter fileStream = new StreamWriter(fs, Encoding.UTF8)
            {
                AutoFlush = true
            };

            return fileStream;
        }

        private void StartLog()
        {
            _fileStream?.WriteLine($"\r\n>>>>>>>>>>>>>> Log started {DateTime.Now.ToString("yyyyMMdd hh:mm:ss.fff")}");
        }

        private bool CheckLogFileInUse()
        {
            FileStream stream = null;
            try
            {
                stream = new FileStream(_fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
            
            return false;
        }

        public async Task WriteAndFlushAsync(string data)
        {
            await semaphoreSlim.WaitAsync();
            try
            {
                await _fileStream.WriteLineAsync(data);
                await _fileStream.FlushAsync();
            }
            catch (Exception exception)
            {
                Trace.TraceError($"Error writing to the stream. Exception:\r\n{exception.ToString()}");
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }

        private static void PurgeOldLogs(string logFolder, int daysToKeep)
        {
            if (Directory.Exists(logFolder))
            {
                DirectoryInfo di = new DirectoryInfo(logFolder);
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



        ~FileHealthWriter()
        {
            Dispose(false);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources 
                if (_fileStream != null)
                {
                    _fileStream.Dispose();
                }
            }
            //free native resources if any.
        }
    }
}
