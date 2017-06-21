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
using System.Reflection;

using EnvDTE;
using EnvDTE80;

using Microsoft.Templates.UI.Resources;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;

namespace Microsoft.Templates.UI.VisualStudio
{
    class VsOutputPane
    {
        private const string TemplatesPaneGuid = "45480fff-0658-42e1-97f0-82cac23603aa";
        private OutputWindowPane _pane;

        public VsOutputPane()
        {
            _pane = GetOrCreatePane(Guid.Parse(TemplatesPaneGuid), true, false);

            if (_pane != null)
            {
                _pane.Activate();
            }
        }
        public void Write(string data)
        {
            _pane.OutputString(data);
        }

        private static OutputWindowPane GetOrCreatePane(Guid paneGuid, bool visible, bool clearWithSolution)
        {
            OutputWindowPane result = null;

            try
            {
                if (ServiceProvider.GlobalProvider.GetService(typeof(DTE)) is DTE2 dte)
                {
                    result = GetUwpPane(dte, paneGuid);

                    if (result == null)
                    {
                        CreateUwpPane(paneGuid, visible, clearWithSolution, StringRes.Title);
                        result = GetUwpPane(dte, paneGuid);
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Exception creating Visual Studio Output window pane. {ex.ToString()}");
            }

            return result;
        }

        private static void CreateUwpPane(Guid paneGuid, bool visible, bool clearWithSolution, string title)
        {
            IVsOutputWindow output = ServiceProvider.GlobalProvider.GetService(typeof(SVsOutputWindow)) as IVsOutputWindow;
            // Create a new pane.
            int createRetVal = output.CreatePane(
                ref paneGuid,
                title,
                Convert.ToInt32(visible),
                Convert.ToInt32(clearWithSolution));

            output.GetPane(ref paneGuid, out var pane);
            pane.OutputString($"Windows Template Studio {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}\n");
            pane.OutputString($"Version: {GetVersion()}\n");
            pane.OutputString($">\n");
        }

        private static string GetVersion()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            return assembly.GetName().Version.ToString();
        }

        private static OutputWindowPane GetUwpPane(DTE2 dte,  Guid uwpOutputPaneGuid)
        {
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
