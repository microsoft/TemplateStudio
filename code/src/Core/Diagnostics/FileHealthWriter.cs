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

        public FileHealthWriter(Configuration currentConfig)
        {        
            _workingFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), currentConfig.LogFileFolderPath);

            //TODO: Purge files older than 5 days.
            PurgeOldLogs();
        }

        public async Task WriteTraceAsync(TraceEventType eventType, string message, Exception ex=null)
        {
            await InitializeLogFile();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{FormattedWriterMessages.LogEntryStart}\t{eventType.ToString():11}\t{message}");
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

            await InitializeLogFile();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{FormattedWriterMessages.LogEntryStart}\t{TraceEventType.Critical.ToString():11}\tException Tracked. {(message != null ? message : "")}");
            if (ex != null)
            {
                sb.AppendLine(FormattedWriterMessages.ExHeader);
                sb.AppendLine(ex.ToString());
                sb.AppendLine(FormattedWriterMessages.ExFooter);
            }

            await WriteAndFlushAsync(sb.ToString());
        }

        private async Task InitializeLogFile()
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

        private void PurgeOldLogs()
        {
            DirectoryInfo di = new DirectoryInfo(_workingFolder);
            di.GetFiles().Where(f => f.CreationTimeUtc >= DateTime.UtcNow.AddDays(5));
            //TODO: End
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
