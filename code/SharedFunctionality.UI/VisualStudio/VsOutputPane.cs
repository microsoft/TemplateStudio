// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using EnvDTE;
using EnvDTE80;
using Microsoft.Templates.Core;
using Microsoft.Templates.Resources;
using Microsoft.Templates.UI.Threading;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Task = System.Threading.Tasks.Task;

namespace Microsoft.Templates.UI.VisualStudio
{
    internal class VsOutputPane
    {
#if _UWP_
        private const string TemplatesPaneGuid = "24d5b7eb-4036-4338-bdd0-efa1f0c88b0e";
#elif _WPF_
        private const string TemplatesPaneGuid = "cc84c96e-ded0-4a55-8293-18830413d8c9";
#elif _WINUICS_
        private const string TemplatesPaneGuid = "ea91557d-4baf-467a-acb7-8796928eaf71";
#elif _WINUICPP_
        private const string TemplatesPaneGuid = "342605f0-d1cf-40cf-8aa6-ed3912f97b9f";
#elif _TEST_
        // This one enables tests to build (& run)
        private const string TemplatesPaneGuid = "0a914691-adb5-48d0-908e-58479f8cdbd3";
#else
#warning Use one of DebugXXX or ReleaseXXX (to get appropriate TemplatesPaneGuid)
#endif
        private OutputWindowPane _pane;

        public VsOutputPane()
        {
        }

        public async Task InitializeAsync()
        {
            await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();

            _pane = await GetOrCreatePaneAsync(Guid.Parse(TemplatesPaneGuid), true, false);

            if (_pane != null)
            {
                _pane.Activate();
            }
        }

        public void Write(string data)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            _pane.OutputString(data);
        }

        private static async Task<OutputWindowPane> GetOrCreatePaneAsync(Guid paneGuid, bool visible, bool clearWithSolution)
        {
            await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();

            OutputWindowPane result = null;

            try
            {
                if (ServiceProvider.GlobalProvider.GetService(typeof(DTE)) is DTE2 dte)
                {
                    result = await GetUwpPaneAsync(dte, paneGuid);

                    if (result == null)
                    {
                        await CreateUwpPaneAsync(paneGuid, visible, clearWithSolution, TemplateStudioProject.AppName);
                        result = await GetUwpPaneAsync(dte, paneGuid);
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Exception creating Visual Studio Output window pane. {ex}");
            }

            return result;
        }

        private static async Task CreateUwpPaneAsync(Guid paneGuid, bool visible, bool clearWithSolution, string title)
        {
            await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
            if (ServiceProvider.GlobalProvider.GetService(typeof(SVsOutputWindow)) is IVsOutputWindow output)
            {
                // Create a new pane.
                output.CreatePane(
                    ref paneGuid,
                    title,
                    Convert.ToInt32(visible),
                    Convert.ToInt32(clearWithSolution));

                output.GetPane(ref paneGuid, out var pane);

                pane.OutputStringThreadSafe($"{TemplateStudioProject.AppName} - {DateTime.Now.FormatAsFullDateTime()}\n");
                pane.OutputStringThreadSafe($"Version: {GetVersion()}\n");
                pane.OutputStringThreadSafe($">\n");
            }
        }

        private static string GetVersion()
        {
            return Core.Gen.GenContext.GetWizardVersionFromAssembly().ToString();
        }

        private static async Task<OutputWindowPane> GetUwpPaneAsync(DTE2 dte, Guid uwpOutputPaneGuid)
        {
            await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();

            OutputWindowPanes panes = dte.ToolWindows.OutputWindow.OutputWindowPanes;
            OutputWindowPane result = null;

            foreach (OutputWindowPane p in panes)
            {
                if (Guid.Parse(p.Guid).ToString() == uwpOutputPaneGuid.ToString())
                {
                    result = p;
                }
            }

            return result;
        }
    }
}
