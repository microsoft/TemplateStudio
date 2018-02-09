// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Fakes;
using Microsoft.Templates.UI;
using Microsoft.Templates.UI.Services;
using Microsoft.Templates.UI.Threading;
using Microsoft.Templates.VsEmulator.Services;
using Microsoft.VisualStudio.TemplateWizard;

namespace Microsoft.Templates.VsEmulator
{
    public partial class App : Application
    {
        public App()
        {
        }

        public static void LoadDarkTheme()
        {
            Application.Current.Resources.MergedDictionaries.Clear();
            MergeDictionary("VsTheme.xaml");
            MergeDictionary("Dark_CommonControls.xaml");
            MergeDictionary("Dark_CommonDocument.xaml");
            MergeDictionary("Dark_Environment.xaml");
            MergeDictionary("Dark_InfoBar.xaml");
            MergeDictionary("Dark_ThemedDialog.xaml");
            MergeDictionary("Dark_WindowsTemplateStudio.xaml");
        }

        public static void LoadLightTheme()
        {
            Application.Current.Resources.MergedDictionaries.Clear();
            MergeDictionary("VsTheme.xaml");
            MergeDictionary("Light_CommonControl.xaml");
            MergeDictionary("Light_CommonDocument.xaml");
            MergeDictionary("Light_Environment.xaml");
            MergeDictionary("Light_InfoBar.xaml");
            MergeDictionary("Light_ThemedDialog.xaml");
            MergeDictionary("Light_WindowsTemplateStudio.xaml");
        }

        private static void MergeDictionary(string dictionaryName)
        {
            var rd = new ResourceDictionary();
            var stringUrl = $"/VsEmulator;component/Styles/{dictionaryName}";
            rd.Source = new Uri(stringUrl, UriKind.RelativeOrAbsolute);
            Application.Current.Resources.MergedDictionaries.Add(rd);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (e.Args.Any())
            {
                LaunchWizardFromCommandLineForAutomatedTesting(e.Args);
            }
        }

        private void LaunchWizardFromCommandLineForAutomatedTesting(string[] args)
        {
            SafeThreading.JoinableTaskFactory.Run(async () =>
            {
                var exitCode = 0;

                await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
                try
                {
                    var options = new CommandLineOptions();

                    if (CommandLine.Parser.Default.ParseArguments(args, options))
                    {
                        var cultureArg = options.Culture;

                        if (!string.IsNullOrWhiteSpace(cultureArg))
                        {
                            var culture = new CultureInfo(cultureArg);

                            Thread.CurrentThread.CurrentCulture = culture;
                            Thread.CurrentThread.CurrentUICulture = culture;
                        }

                        var progLanguage = options.ProgLang;

                        var newProjectName = string.IsNullOrWhiteSpace(options.ProjectName)
                            ? Path.GetFileName(Path.GetTempFileName().Replace(".", string.Empty))
                            : options.ProjectName;

                        // TODO [ML]: add support for right-click wizards too.
                        GenContext.Bootstrap(
                            new LocalTemplatesSource("0.0.0.0", string.Empty),
                            new FakeGenShell(progLanguage),
                            new Version("0.0.0.0"),
                            progLanguage);

                        await GenContext.ToolBox.Repo.RefreshAsync();

                        GenContext.SetCurrentLanguage(progLanguage);
                        var fakeShell = GenContext.ToolBox.Shell as FakeGenShell;
                        fakeShell?.SetCurrentLanguage(progLanguage);

                        var newProjectLocation = Path.GetTempPath();

                        var projectPath = Path.Combine(newProjectLocation, newProjectName, newProjectName);

                        var context = new FakeContextProvider
                        {
                            ProjectName = newProjectName,
                            ProjectPath = projectPath,
                            OutputPath = projectPath
                        };

                        GenContext.Current = context;

                        App.LoadLightTheme();
                        UIStylesService.Instance.Initialize(new FakeStyleValuesProvider());
                        var userSelectionIsNotUsed = NewProjectGenController.Instance.GetUserSelection(progLanguage);
                    }
                    else
                    {
                        try
                        {
                            if (!NativeMethods.AttachConsole(-1))
                            {
                                // allocate a new console
                                NativeMethods.AllocConsole();
                            }

                            Console.WriteLine(options.GetUsage());
                            Console.ReadKey(true);
                        }
                        finally
                        {
                            NativeMethods.FreeConsole();
                            exitCode = -1;
                        }
                    }
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
    }
}
