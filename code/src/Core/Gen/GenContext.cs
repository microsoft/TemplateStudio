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
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Resources;
using Microsoft.Templates.Core.Locations;

namespace Microsoft.Templates.Core.Gen
{
    public class GenContext
    {
        private static IContextProvider _currentContext;

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
            AppHealth.Current.AddWriter(new ShellHealthWriter());
            AppHealth.Current.Info.TrackAsync($"{StringRes.ConfigurationFileLoadedString}: {Configuration.LoadedConfigFile}").FireAndForget();

            string hostVersion = $"{wizardVersion.Major}.{wizardVersion.Minor}";

            CodeGen.Initialize(source.Id, hostVersion);
            var repository = new TemplatesRepository(source, wizardVersion);

            ToolBox = new GenToolBox(repository, shell);

            PurgeTempGenerations(Path.Combine(Path.GetTempPath(), Configuration.Current.TempGenerationFolderPath), Configuration.Current.DaysToKeepTempGenerations);

            IsInitialized = true;
        }

        private static void PurgeTempGenerations(string tempGenerationFolder, int daysToKeep)
        {
            if (Directory.Exists(tempGenerationFolder))
            {
                var di = new DirectoryInfo(tempGenerationFolder);
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
