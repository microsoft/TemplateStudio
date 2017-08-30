// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Templates.Core.Gen;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Win32;
using Microsoft.Templates.UI.VisualStudio;

namespace Microsoft.Templates.Extension.Commands
{
    [ProvideService((typeof(ISGenContextBootstrapService)), IsAsyncQueryable = true)]
    [ProvideAutoLoad("{f1536ef8-92ec-443c-9ed7-fdadf150da82}", PackageAutoLoadFlags.BackgroundLoad)]
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(PackageGuids.PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    public sealed class RelayCommandPackage : AsyncPackage
    {
        private readonly Lazy<RightClickActions> _rightClickActions = new Lazy<RightClickActions>(() => new RightClickActions(GenContext.ToolBox.Shell.GetActiveProjectLanguage()));
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

            IGenContextBootstrapService bootstrapsvc = await PrepareBootstrapSvcAsync();

            var shell = new VsGenShell();

            var language = shell.GetActiveProjectLanguage();

            await bootstrapsvc.GenContextInitAsync(shell, language);

            InitializeCommands();

            await base.InitializeAsync(cancellationToken, progress);
        }

        private async Task<IGenContextBootstrapService> PrepareBootstrapSvcAsync()
        {
            AddService(typeof(ISGenContextBootstrapService), CreateServiceAsync);
            IGenContextBootstrapService bootstrapsvc = await GetServiceAsync(typeof(ISGenContextBootstrapService)) as IGenContextBootstrapService;

            return bootstrapsvc;
        }

        private async Task<object> CreateServiceAsync(IAsyncServiceContainer container, CancellationToken cancellationToken, Type serviceType)
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
            if (RightClickActions.Visible())
            {
                RightClickActions.AddNewPage();
            }
        }

        private void AddFeature(object sender, EventArgs e)
        {
            if (RightClickActions.Visible())
            {
                RightClickActions.AddNewFeature();
            }
        }

        private void OpenTempFolder(object sender, EventArgs e)
        {
            if (RightClickActions.TempFolderAvailable() && RightClickActions.Visible())
            {
                RightClickActions.OpenTempFolder();
            }
        }

        private void RightClickAvailable(object sender, EventArgs e)
        {
            var cmd = (OleMenuCommand)sender;
            cmd.Enabled = RightClickActions.Enabled();
            cmd.Visible = RightClickActions.Visible();
        }

        private void TempFolderAvailable(object sender, EventArgs e)
        {
            var cmd = (OleMenuCommand)sender;
            cmd.Visible = RightClickActions.TempFolderAvailable() && RightClickActions.Visible();
        }
    }
}
