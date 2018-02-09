// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;

using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Core.PostActions.Catalog.Merge;
using Microsoft.Templates.Fakes;
using Microsoft.Templates.UI;
using Microsoft.Templates.UI.Services;
using Microsoft.Templates.UI.Threading;
using Microsoft.VisualStudio.TemplateWizard;

namespace TestWizardLauncher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, IContextProvider
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            SafeThreading.JoinableTaskFactory.Run(async () =>
            {
                var exitCode = 0;

                await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
                try
                {
                    var cultureArg = e.Args.FirstOrDefault();

                    if (!string.IsNullOrWhiteSpace(cultureArg))
                    {
                        var culture = new CultureInfo(cultureArg);

                        Thread.CurrentThread.CurrentCulture = culture;
                        Thread.CurrentThread.CurrentUICulture = culture;
                    }

                    var progLanguage = ProgrammingLanguages.CSharp;

                    if (e.Args.Length > 1)
                    {
                        progLanguage = e.Args[1] == ProgrammingLanguages.VisualBasic
                            ? ProgrammingLanguages.VisualBasic
                            : ProgrammingLanguages.CSharp;
                    }

                    var newProjectName = Path.GetFileName(Path.GetTempFileName().Replace(".", string.Empty));  // Make longer by including the extension

                    if (e.Args.Length > 2)
                    {
                        newProjectName = e.Args[2];
                    }

                    GenContext.Bootstrap(new LocalTemplatesSource("0.0.0.0", string.Empty), new FakeGenShell(progLanguage), new Version("0.0.0.0"), progLanguage);

                    await GenContext.ToolBox.Repo.RefreshAsync();

                    GenContext.SetCurrentLanguage(progLanguage);
                    var fakeShell = GenContext.ToolBox.Shell as FakeGenShell;
                    fakeShell?.SetCurrentLanguage(progLanguage);

                    var newProjectLocation = Path.GetTempPath();

                    var projectPath = Path.Combine(newProjectLocation, newProjectName, newProjectName);
                    GenContext.Current = this;
                    ProjectName = newProjectName;
                    ProjectPath = projectPath;
                    OutputPath = projectPath;
                    UIStylesService.Instance.Initialize(new FakeStyleValuesProvider());
                    var userSelectionIsNotUsed = NewProjectGenController.Instance.GetUserSelection(progLanguage);
                }
                catch (WizardBackoutException)
                {
                    // Get this if cancel out of the wizard
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, "Error");
                    exitCode = exception.HResult;
                }

                Application.Current.Shutdown(exitCode);
            });
        }

        public string ProjectName { get; private set; }

        public string OutputPath { get; private set; }

        public string ProjectPath { get; private set; }

        public List<string> ProjectItems { get; }

        public List<string> FilesToOpen { get; }

        public List<FailedMergePostAction> FailedMergePostActions { get; }

        public Dictionary<string, List<MergeInfo>> MergeFilesFromProject { get; }

        public Dictionary<ProjectMetricsEnum, double> ProjectMetrics { get; }
    }
}
