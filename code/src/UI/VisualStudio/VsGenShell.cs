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
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;

using EnvDTE;

using Microsoft.Internal.VisualStudio.PlatformUI;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Gen;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TemplateWizard;
using Microsoft.Templates.UI.Resources;

using NuGet.VisualStudio;

namespace Microsoft.Templates.UI.VisualStudio
{
    public class VsGenShell : GenShell
    {
        private Lazy<DTE> _dte = new Lazy<DTE>(() => ServiceProvider.GlobalProvider.GetService(typeof(DTE)) as DTE, true);
        private DTE Dte => _dte.Value;

        private Lazy<IVsUIShell> _uiShell = new Lazy<IVsUIShell>(() => ServiceProvider.GlobalProvider.GetService(typeof(SVsUIShell)) as IVsUIShell, true);
        private IVsUIShell UIShell => _uiShell.Value;

        private Lazy<VsOutputPane> _outputPane = new Lazy<VsOutputPane>(() => new VsOutputPane());
        private VsOutputPane OutputPane => _outputPane.Value;

        public override void AddItems(params string[] itemsFullPath)
        {
            if (itemsFullPath == null || itemsFullPath.Length == 0)
            {
                return;
            }

            var proj = GetActiveProject();
            if (proj != null && proj.ProjectItems != null)
            {
                foreach (var item in itemsFullPath)
                {
                    proj.ProjectItems.AddFromFile(item);
                }

                proj.Save();
            }
            else
            {
                AppHealth.Current.Error.TrackAsync(StringRes.UnableAddItemsToProject).FireAndForget();
            }
        }

        public override void RefreshProject()
        {
            try
            {
                var proj = GetActiveProject();

                if (proj != null)
                {
                    var path = proj.FullName;

                    Dte.Solution.Remove(proj);
                    Dte.Solution.AddFromFile(path);
                }
            }
            catch(Exception)
            {
                //WE GET AN EXCEPTION WHEN THERE ISN'T A PROJECT LOADED
                AppHealth.Current.Info.TrackAsync(StringRes.UnableToRefreshProject).FireAndForget();
            }
        }

        public override bool SetActiveConfigurationAndPlatform(string configurationName, string platformName)
        {
            foreach (SolutionConfiguration solConfiguration in Dte.Solution.SolutionBuild.SolutionConfigurations)
            {
                if (solConfiguration.Name == configurationName)
                {
                    foreach (SolutionContext context in solConfiguration.SolutionContexts)
                    {
                        if (context.PlatformName == platformName)
                        {
                            solConfiguration.Activate();

                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public override void AddProjectToSolution(string projectFullPath)
        {
            try
            {
                Dte.Solution.AddFromFile(projectFullPath);
            }
            catch (Exception)
            {
                //WE GET AN EXCEPTION WHEN THERE ISN'T A SOLUTION LOADED
                AppHealth.Current.Info.TrackAsync(StringRes.UnableAddProjectToSolution).FireAndForget();
            }
        }

        public override string GetActiveNamespace()
        {
            if (Dte.SelectedItems.Count > 0)
            {
                var selectedItem = Dte.SelectedItems.Item(1);

                if (selectedItem.Project != null)
                {
                    return selectedItem.Project.Properties.GetSafeValue("DefaultNamespace");
                }

                if (selectedItem.ProjectItem != null)
                {
                    return $"{selectedItem.ProjectItem.Properties.GetSafeValue("DefaultNamespace")}";
                }
            }

            var p = GetActiveProject();

            if (p != null)
            {
                return p.Properties.GetSafeValue("DefaultNamespace");
            }

            return null;
        }

        public override void SaveSolution(string solutionFullPath)
        {
            Dte.Solution.SaveAs(solutionFullPath);
        }

        public override void ShowStatusBarMessage(string message)
        {
            try
            {
                Dte.StatusBar.Text = message;
            }
            catch (Exception ex)
            {
                AppHealth.Current.Error.TrackAsync($"There was an error showing status message. Ex: {ex.ToString()}").FireAndForget();
            }
        }

        public override void ShowTaskList()
        {
            ShowTaskListAsync().FireAndForget();
        }

        public override void ShowModal(System.Windows.Window dialog)
        {
            //get the owner of this dialog
            IntPtr hwnd;

            UIShell.GetDialogOwnerHwnd(out hwnd);

            dialog.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;

            UIShell.EnableModeless(0);

            try
            {
                WindowHelper.ShowModal(dialog, hwnd);
            }
            finally
            {
                // This will take place after the window is closed.
                UIShell.EnableModeless(1);
            }
        }

        public override void CancelWizard(bool back = true)
        {
            if (back)
            {
                throw new WizardBackoutException();
            }
            else
            {
                throw new WizardCancelledException();
            }
        }

        protected override string GetSelectedItemPath()
        {
            if (Dte.SelectedItems.Count > 0)
            {
                var item = Dte.SelectedItems.Item(1);

                if (item.Project != null)
                {
                    return Path.GetDirectoryName(item.Project.FullName);
                }
                if (item.ProjectItem != null)
                {
                    string fullPath = $"{item.ProjectItem.Properties.GetSafeValue("FullPath")}";

                    return Path.GetDirectoryName(fullPath);
                }
            }

            return GetActiveProjectPath();
        }

        protected override string GetActiveProjectName()
        {
            var p = GetActiveProject();

            return p?.Name;
        }

        protected override string GetActiveProjectPath()
        {
            var p = GetActiveProject();

            if (p != null)
            {
                return Path.GetDirectoryName(p.FileName);
            }
            else
            {
                return null;
            }
        }

        private Project GetActiveProject()
        {
            Project p = null;

            try
            {
                if (_dte != null)
                {
                    Array projects = (Array)Dte.ActiveSolutionProjects;

                    if (projects?.Length >= 1)
                    {
                        p = (Project)projects.GetValue(0);
                    }
                }
            }
            catch (Exception)
            {
                //WE GET AN EXCEPTION WHEN THERE ISN'T A PROJECT LOADED
            }

            return p;
        }

        private async System.Threading.Tasks.Task ShowTaskListAsync()
        {
            //JAVIERS: DELAY THIS EXECUTION TO OPEN THE WINDOW AFTER EVERYTHING IS LOADED
            await System.Threading.Tasks.Task.Delay(1000);

            var window = Dte.Windows.Item(EnvDTE.Constants.vsWindowKindTaskList);

            window.Activate();
        }

        public override void WriteOutput(string data)
        {
            OutputPane.Write(data);
        }

        public override void CloseSolution()
        {
            Dte.Solution.Close();
        }

        public override void RestorePackages()
        {
            try
            {
                if (IsInternetAvailable())
                {
                    var componentModel = (IComponentModel)Package.GetGlobalService(typeof(SComponentModel));

                    var installer = componentModel.GetService<IVsPackageInstaller>();
                    var uninstaller = componentModel.GetService<IVsPackageUninstaller>();
                    var installerServices = componentModel.GetService<IVsPackageInstallerServices>();

                    var installedPackages = installerServices.GetInstalledPackages().ToList();
                    var activeProject = GetActiveProject();

                    var p = installedPackages.FirstOrDefault();

                    if (p != null)
                    {
                        uninstaller.UninstallPackage(activeProject, p.Id, false);
                        installer.InstallPackage("All", activeProject, p.Id, p.VersionString, true);
                    }
                }
                else
                {
                    AppHealth.Current.Warning.TrackAsync("Unable to automatically perform Restore NuGet Packages for the solution. Please, try to manually restore the NuGet packages.").FireAndForget();
                }
            }
            catch (Exception ex)
            {
                AppHealth.Current.Error.TrackAsync($"There was an error restoring the packages. Ex: {ex.ToString()}").FireAndForget();
            }
        }

     
        public override void CollapseSolutionItems()
        {
            try
            {
                var solutionExplorer = Dte.Windows.Item(EnvDTE.Constants.vsext_wk_SProjectWindow).Object as UIHierarchy;
                var projectNode = solutionExplorer.UIHierarchyItems.Item(1)?.UIHierarchyItems.Item(1);

                foreach (UIHierarchyItem item in projectNode.UIHierarchyItems)
                {
                    Collapse(item);
                }
            }
            catch (Exception ex)
            {
                AppHealth.Current.Error.TrackAsync($"There was an error collapsing the solution tree. Ex: {ex.ToString()}").FireAndForget();
            }
        }

        public override string GetVsCultureInfo()
        {
            return System.Globalization.CultureInfo.GetCultureInfo(Dte.LocaleID).Name;
        }

        public override string GetVsVersion()
        {
            return Dte.Version;
        }

        public override string GetVsEdition()
        {
            return Dte.Edition;
        }

        private void Collapse(UIHierarchyItem item)
        {
            foreach (UIHierarchyItem subitem in item.UIHierarchyItems)
            {
                Collapse(subitem);
            }

            item.UIHierarchyItems.Expanded = false;
        }

        private static bool IsInternetAvailable()
        {
            bool internet = NetworkInterface.GetIsNetworkAvailable();
            if (internet)
            {
                try
                {
                    //Based on https://technet.microsoft.com/en-us/library/cc766017(v=ws.10).aspx 
                    using (var client = new System.Net.WebClient())
                    {
                        var ncsi = client.DownloadString("http://www.msftncsi.com/ncsi.txt");
                        internet = (ncsi == "Microsoft NCSI");
                    }
                }
                catch
                {
                    internet = false;
                }
            }
            return internet;
        }


    }
}
