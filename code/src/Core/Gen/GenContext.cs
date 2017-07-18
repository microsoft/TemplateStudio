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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Resources;
using Microsoft.Templates.Core.Locations;

namespace Microsoft.Templates.Core.Gen
{
    public class GenContext
    {
        private static IContextProvider _currentContext;
        private static string _tempGenerationFolder = Path.Combine(Path.GetTempPath(), Configuration.Current.TempGenerationFolderPath);

        public static GenToolBox ToolBox { get; private set; }
        public static bool IsInitialized { get; private set; }

        public static IContextProvider Current
        {
            get
            {
                if (_currentContext == null)
                {
                    throw new InvalidOperationException(StringRes.GenContextCurrentInvalidOperationMessage);
                }
                return _currentContext;
            }
            set
            {
                _currentContext = value;
            }
        }

        public static void Bootstrap(TemplatesSource source, GenShell shell)
        {
            Bootstrap(source, shell, GetWizardVersionFromAssembly());
        }

        public static void Bootstrap(TemplatesSource source, GenShell shell, Version wizardVersion)
        {
            try
            {
                AppHealth.Current.AddWriter(new ShellHealthWriter(shell));
                AppHealth.Current.Info.TrackAsync($"{StringRes.ConfigurationFileLoadedString}: {Configuration.LoadedConfigFile}").FireAndForget();

                string hostVersion = $"{wizardVersion.Major}.{wizardVersion.Minor}";

                var repository = new TemplatesRepository(source, wizardVersion);

                ToolBox = new GenToolBox(repository, shell);

                PurgeTempGenerations(Configuration.Current.DaysToKeepTempGenerations);

                CodeGen.Initialize(source.Id, hostVersion);

                IsInitialized = true;
            }
            catch (Exception ex)
            {
                IsInitialized = false;
                AppHealth.Current.Exception.TrackAsync(ex, StringRes.GenContextBootstrapError).FireAndForget();
                Trace.TraceError($"{StringRes.GenContextBootstrapError} Exception:\n\r{ex}");
                throw;
            }
        }

        public static string GetTempGenerationPath(string projectName)
        {
            Fs.EnsureFolder(_tempGenerationFolder);

            var tempGenerationName = $"{projectName}_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}";
            var inferredName = Naming.Infer(tempGenerationName, new List<Validator>() { new DirectoryExistsValidator(_tempGenerationFolder) }, "_");

            return Path.Combine(_tempGenerationFolder, inferredName);
        }

        private static void PurgeTempGenerations(int daysToKeep)
        {
            if (Directory.Exists(_tempGenerationFolder))
            {
                var di = new DirectoryInfo(_tempGenerationFolder);
                var toBeDeleted = di.GetDirectories().Where(d => d.CreationTimeUtc.AddDays(daysToKeep) < DateTime.UtcNow);

                foreach (var d in toBeDeleted)
                {
                    try
                    {
                        d.Delete(true);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error removing old temp generation directory '{d.FullName}'. Skipped. Exception:\n\r{ex.ToString()}");
                        Trace.TraceError($"Error removing old temp generation directory '{d.FullName}'. Skipped. Exception:\n\r{ex.ToString()}");
                    }
                }
            }
        }

        private static Version GetWizardVersionFromAssembly()
        {
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            var versionInfo = FileVersionInfo.GetVersionInfo(assemblyLocation);

            Version.TryParse(versionInfo.FileVersion, out Version v);

            return v;
        }
    }
}
