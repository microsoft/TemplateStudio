// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Threading;

using Microsoft.Templates.UI.Threading;
using Microsoft.Templates.UI.VisualStudio;
using Microsoft.VisualStudio.Shell;

namespace Microsoft.Templates.Extension.Commands
{
    [ProvideAutoLoad(Microsoft.VisualStudio.VSConstants.UICONTEXT.SolutionHasAppContainerProject_string, PackageAutoLoadFlags.BackgroundLoad)]
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration("#110", "#112", "0.0.0.0", IconResourceID = 400)] // Info on this package for Help/About
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
            await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();

            InitializeCommands();

            await base.InitializeAsync(cancellationToken, progress);
        }

        private void InitializeCommands()
        {
            addPageCommand = new RelayCommand(
                 this,
                 PackageIds.AddPageCommand,
                 PackageGuids.GuidRelayCommandPackageCmdSet,
                 AddPage,
                 RightClickAvailable);

            addFeatureCommand = new RelayCommand(
                this,
                PackageIds.AddFeatureCommand,
                PackageGuids.GuidRelayCommandPackageCmdSet,
                AddFeature,
                RightClickAvailable);

            openTempFolderCommand = new RelayCommand(
                this,
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
