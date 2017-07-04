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
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Win32;
using Microsoft.Templates.UI.VisualStudio;

namespace Microsoft.Templates.Extension.Commands
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [ProvideService((typeof(ISGenContextBootstrapService)), IsAsyncQueryable = true)]
    [ProvideAutoLoad("{f1536ef8-92ec-443c-9ed7-fdadf150da82}", PackageAutoLoadFlags.BackgroundLoad)]
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(PackageGuids.PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    public sealed class RelayCommandPackage : AsyncPackage
    {
        private readonly Lazy<RightClickActions> _rightClickActions = new Lazy<RightClickActions>(() => new RightClickActions());
        private RightClickActions RightClickActions => _rightClickActions.Value;

        private RelayCommand addPageCommand;
        private RelayCommand addFeatureCommand;
        private RelayCommand openTempFolderCommand;

        public RelayCommandPackage()
        {
        }

        protected override async System.Threading.Tasks.Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            IGenContextBootstrapService bootstrapsvc = await PrepareBootstrapSvc();
            await bootstrapsvc.GenContextInit();

            InitializeCommands();

            await base.InitializeAsync(cancellationToken, progress);
        }

        private async Task<IGenContextBootstrapService> PrepareBootstrapSvc()
        {
            this.AddService(typeof(ISGenContextBootstrapService), CreateService);
            IGenContextBootstrapService bootstrapsvc = await this.GetServiceAsync(typeof(ISGenContextBootstrapService)) as IGenContextBootstrapService;
            return bootstrapsvc;
        }

        private async Task<object> CreateService(IAsyncServiceContainer container, CancellationToken cancellationToken, Type serviceType)
        {
            ISGenContextBootstrapService service = null;
            await System.Threading.Tasks.Task.Run(() =>
            {
                service = new GenContextBootstrapService(this);
            });

            return service;
        }

        private void InitializeCommands()
        {
            addPageCommand = new RelayCommand(this,
                 PackageIds.AddPageCommand,
                 PackageGuids.GuidRelayCommandPackageCmdSet,
                 AddPage,
                 RightClickAvailable);

            addFeatureCommand = new RelayCommand(this,
                PackageIds.AddFeatureCommand,
                PackageGuids.GuidRelayCommandPackageCmdSet,
                AddFeature,
                RightClickAvailable);

            openTempFolderCommand = new RelayCommand(this,
                PackageIds.OpenTempFolder,
                PackageGuids.GuidRelayCommandPackageCmdSet,
                OpenTempFolder,
                TempFolderAvailable);
        }

        private void AddPage(object sender, EventArgs e)
        {
            RightClickActions.AddNewPage();
        }

        private void AddFeature(object sender, EventArgs e)
        {
            RightClickActions.AddNewFeature();
        }

        private void OpenTempFolder(object sender, EventArgs e)
        {
            RightClickActions.OpenTempFolder();
        }

        private void RightClickAvailable(object sender, EventArgs e)
        {
            var cmd = (OleMenuCommand)sender;
            cmd.Visible = RightClickActions.Enabled();
        }

        private void TempFolderAvailable(object sender, EventArgs e)
        {
            var cmd = (OleMenuCommand)sender;
            cmd.Visible = RightClickActions.TempFolderAvailable();
        }
    }
}
