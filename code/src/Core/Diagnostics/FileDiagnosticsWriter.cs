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
    public class FileDiagnosticsWriter: IDiagnosticsWriter, IDisposable
    {
        private const string FolderName = @"UWPTemplates\Logs";
        private readonly Lazy<string> _lazyWorkingFolder = new Lazy<string>(() => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), FolderName));
        private string _workingFolder => _lazyWorkingFolder.Value;
        private string _username;
        private string _processId;

        private StreamWriter _fileStream;
        private string _fileName;
        public string LogFileName
        {
            get
            {
                return _fileName;
            }
        }

        static FileDiagnosticsWriter _default;
        public static FileDiagnosticsWriter Default
        {
            get
            {
                if(_default == null)
                {
                    _default = new FileDiagnosticsWriter();
                }
                return _default;
            }
        }

        static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

        public FileDiagnosticsWriter()
        {
            _username = System.Environment.UserName;
            _processId = $"{Process.GetCurrentProcess().Id.ToString()}";

            InitializeLogFile();
        }

        public async Task WriteEventAsync(TraceEventType eventType, string message, Exception ex=null)
        {
            await semaphoreSlim.WaitAsync();
            try
            {
                await _fileStream?.WriteLineAsync($"{DateTime.Now.ToString("yyyyMMdd hh:mm:ss.fff")}\t{_username}\t{_processId}({System.Threading.Thread.CurrentThread.ManagedThreadId})\t{eventType.ToString()}\t{message}{(ex != null ? "\tException Details:" : "")}");
                if (ex != null)
                {
                    await _fileStream?.WriteLineAsync(ex.ToString());
                    await _fileStream?.WriteLineAsync("--");
                }
                await _fileStream.FlushAsync();
            }
            catch (Exception exception)
            {
                Trace.TraceError($"Error creating FileDiagnosticListener. Exception:\r\n{exception.ToString()}");
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }

        public async Task WriteExceptionAsync(Exception ex, string message = null)
        {
            await semaphoreSlim.WaitAsync();
            try
            {
                string messageToTrack = message != null ? message : "Exception tracked"; 
                await _fileStream?.WriteLineAsync($"{DateTime.Now.ToString("yyyyMMdd hh:mm:ss.fff")}\t{_username}\t{_processId}({System.Threading.Thread.CurrentThread.ManagedThreadId})\t{TraceEventType.Critical.ToString()}\t{messageToTrack}{(ex != null ? "\tException Details:" : "")}");
                if (ex != null)
                {
                    await _fileStream?.WriteLineAsync(ex.ToString());
                    await _fileStream?.WriteLineAsync("--");
                }
                else
                {
                    await _fileStream?.WriteLineAsync("The exception object is null");
                    await _fileStream?.WriteLineAsync("--");
                }
                await _fileStream.FlushAsync();
            }
            catch (Exception exception)
            {
                Trace.TraceError($"Error creating FileDiagnosticListener. Exception:\r\n{exception.ToString()}");
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }
        private void InitializeLogFile()
        {
            semaphoreSlim.Wait();
            try
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
            catch (Exception exception)
            {
                Trace.TraceError($"Error creating FileDiagnosticListener. Exception:\r\n{exception.ToString()}");
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
                stream = new FileStream(_fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
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
            //file is not in use
            return false;
        }
        ~FileDiagnosticsWriter()
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
                _fileStream?.Dispose();
            }
            //free native resources if any.
        }

    }
}
