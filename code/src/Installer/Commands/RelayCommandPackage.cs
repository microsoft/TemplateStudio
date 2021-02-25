// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Templates.Core;
using Microsoft.Templates.UI.Threading;
using Microsoft.Templates.UI.VisualStudio;
using Microsoft.VisualStudio.Shell;

namespace Microsoft.Templates.Extension.Commands
{
    [ProvideAutoLoad(Microsoft.VisualStudio.VSConstants.UICONTEXT.SolutionHasAppContainerProject_string, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideAutoLoad(Microsoft.VisualStudio.VSConstants.UICONTEXT.SolutionHasMultipleProjects_string, PackageAutoLoadFlags.BackgroundLoad)]
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
        private RelayCommand addServiceCommand;
        private RelayCommand addTestingCommand;
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
                 RightClickAvailable,
                 TemplateType.Page);

            addFeatureCommand = new RelayCommand(
                this,
                PackageIds.AddFeatureCommand,
                PackageGuids.GuidRelayCommandPackageCmdSet,
                AddFeature,
                RightClickAvailable,
                TemplateType.Feature);

            addServiceCommand = new RelayCommand(
                 this,
                 PackageIds.AddServiceCommand,
                 PackageGuids.GuidRelayCommandPackageCmdSet,
                 AddService,
                 RightClickAvailable,
                 TemplateType.Service);

            addTestingCommand = new RelayCommand(
                 this,
                 PackageIds.AddTestingCommand,
                 PackageGuids.GuidRelayCommandPackageCmdSet,
                 AddTesting,
                 RightClickAvailable,
                 TemplateType.Testing);

            openTempFolderCommand = new RelayCommand(
                this,
                PackageIds.OpenTempFolder,
                PackageGuids.GuidRelayCommandPackageCmdSet,
                OpenTempFolder,
                TempFolderAvailable,
                TemplateType.Unspecified);
        }

        private void AddPage(object sender, EventArgs e)
        {
            if (RightClickActions.Visible(TemplateType.Page))
            {
                RightClickActions.AddNewPage();
            }
        }

        private void AddFeature(object sender, EventArgs e)
        {
            if (RightClickActions.Visible(TemplateType.Feature))
            {
                RightClickActions.AddNewFeature();
            }
        }

        private void AddService(object sender, EventArgs e)
        {
            if (RightClickActions.Visible(TemplateType.Service))
            {
                RightClickActions.AddNewService();
            }
        }

        private void AddTesting(object sender, EventArgs e)
        {
            if (RightClickActions.Visible(TemplateType.Testing))
            {
                RightClickActions.AddNewTesting();
            }
        }

        private void OpenTempFolder(object sender, EventArgs e)
        {
            if (RightClickActions.TempFolderAvailable() && RightClickActions.Visible())
            {
                RightClickActions.OpenTempFolder();
            }
        }

        private void RightClickAvailable(object sender, TemplateType templateType)
        {
            var cmd = (OleMenuCommand)sender;
            cmd.Enabled = RightClickActions.Enabled();
            cmd.Visible = RightClickActions.Visible(templateType);
        }

        private void TempFolderAvailable(object sender, TemplateType templateType)
        {
            var cmd = (OleMenuCommand)sender;
            cmd.Visible = RightClickActions.Visible() && RightClickActions.TempFolderAvailable();
        }
    }
}
