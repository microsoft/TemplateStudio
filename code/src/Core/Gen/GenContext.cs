// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Core.Resources;

namespace Microsoft.Templates.Core.Gen
{
    public class GenContext
    {
        private static IContextProvider _currentContext;

        private static string _tempGenerationFolder = Path.Combine(Path.GetTempPath(), Configuration.Current.TempGenerationFolderPath);

        public static GenToolBox ToolBox { get; private set; }

        public static string CurrentLanguage { get; private set; }

        public static bool ContextInitialized => _currentContext != null;

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

        public static void Bootstrap(TemplatesSource source, GenShell shell, string language)
        {
            Bootstrap(source, shell, GetWizardVersionFromAssembly(), language);
        }

        public static void SetCurrentLanguage(string language)
        {
            CurrentLanguage = language;
            ToolBox.Repo.CurrentLanguage = language;
        }

        public static void Bootstrap(TemplatesSource source, GenShell shell, Version wizardVersion, string language)
        {
            try
            {
                AppHealth.Current.AddWriter(new ShellHealthWriter(shell));
                AppHealth.Current.Info.TrackAsync($"{StringRes.ConfigurationFileLoadedString}: {Configuration.LoadedConfigFile}").FireAndForget();

                string hostVersion = $"{shell.GetVsVersionAndInstance()}-{wizardVersion.Major}.{wizardVersion.Minor}";

                CodeGen.Initialize(source.Id, hostVersion);
                var repository = new TemplatesRepository(source, wizardVersion, language);

                ToolBox = new GenToolBox(repository, shell);

                PurgeTempGenerations(Configuration.Current.DaysToKeepTempGenerations);

                CodeGen.Initialize(source.Id, hostVersion);

                CurrentLanguage = language;
            }
            catch (Exception ex)
            {
                AppHealth.Current.Exception.TrackAsync(ex, StringRes.GenContextBootstrapError).FireAndForget();
                Trace.TraceError($"{StringRes.GenContextBootstrapError} Exception:\n\r{ex}");
                throw;
            }
        }

        public static string GetTempGenerationPath(string projectName)
        {
            string projectGuid = ToolBox.Shell.GetVsProjectId().ToString();
            var projectTempFolder = Path.Combine(_tempGenerationFolder, projectGuid);

            Fs.EnsureFolder(projectTempFolder);
            var tempGenerationName = $"{projectName}_{DateTime.Now.FormatAsShortDateTime()}";
            var inferredName = Naming.Infer(tempGenerationName, new List<Validator> { new SuggestedDirectoryNameValidator(projectTempFolder) }, "_");

            return Path.Combine(projectTempFolder, inferredName);
        }

        private static void PurgeTempGenerations(int daysToKeep)
        {
            if (Directory.Exists(_tempGenerationFolder))
            {
                var di = new DirectoryInfo(_tempGenerationFolder);

                var toBeDeleted = di.GetDirectories()
                    .Where(d => Guid.TryParse(d.Name, out Guid guidID))
                    .SelectMany(d => d.GetDirectories())
                    .Where(d => d.CreationTimeUtc.AddDays(daysToKeep) < DateTime.UtcNow);

                foreach (var d in toBeDeleted)
                {
                    Fs.SafeDeleteDirectory(d.FullName);
                    if (!d.Parent.GetDirectories().Any())
                    {
                        Fs.SafeDeleteDirectory(d.Parent.FullName);
                    }
                }
            }
        }

        public static Version GetWizardVersionFromAssembly()
        {
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            var versionInfo = FileVersionInfo.GetVersionInfo(assemblyLocation);

            Version.TryParse(versionInfo.FileVersion, out Version v);

            return v;
        }
    }
}
