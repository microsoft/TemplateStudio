// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Templates.Core;
using Microsoft.Templates.UI.VisualStudio;
using Microsoft.VisualStudio.Shell;
using TemplateStudioForWinUICs.Commands;
using Task = System.Threading.Tasks.Task;

namespace TemplateStudioForWinUICs
{
    [ProvideAutoLoad(PackageGuids.guidTemplateStudioForWinUICsUIContextString, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideUIContextRule(PackageGuids.guidTemplateStudioForWinUICsUIContextString,
       name: "Load TS4WinUI C# Project Package",
       expression: "HasWinUI",
       termNames: new[] { "HasWinUI" },
       termValues: new[] { "SolutionHasProjectCapability:WINUI" })]
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [Guid(PackageGuids.ts4WinuiPackageString)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    public sealed class TemplateStudioForWinUIPackage : AsyncPackage
    {
        private readonly Lazy<RightClickActions> _rightClickActions = new Lazy<RightClickActions>(() => new RightClickActions());

        private RightClickActions RightClickActions => _rightClickActions.Value;

#pragma warning disable IDE0052 // Remove unread private members
        private TemplateStudioCommand addPageCommand;
        private TemplateStudioCommand addFeatureCommand;
        private TemplateStudioCommand addServiceCommand;
        private TemplateStudioCommand addTestingCommand;
        private TemplateStudioCommand openTempFolderCommand;
#pragma warning restore IDE0052 // Remove unread private members

        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            await InitializeCommandsAsync();

            await base.InitializeAsync(cancellationToken, progress);
        }

        private async Task InitializeCommandsAsync()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(DisposalToken);

            OleMenuCommandService commandService = await GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;

            addPageCommand = new TemplateStudioCommand(
                 this,
                 commandService,
                 PackageIds.AddPageCommand,
                 PackageGuids.ts4WinuiPackageCmdSet,
                 AddPage,
                 RightClickAvailable,
                 TemplateType.Page);

            addFeatureCommand = new TemplateStudioCommand(
                this,
                 commandService,
                PackageIds.AddFeatureCommand,
                PackageGuids.ts4WinuiPackageCmdSet,
                AddFeature,
                RightClickAvailable,
                TemplateType.Feature);

            addServiceCommand = new TemplateStudioCommand(
                 this,
                 commandService,
                 PackageIds.AddServiceCommand,
                 PackageGuids.ts4WinuiPackageCmdSet,
                 AddService,
                 RightClickAvailable,
                 TemplateType.Service);

            addTestingCommand = new TemplateStudioCommand(
                 this,
                 commandService,
                 PackageIds.AddTestingCommand,
                 PackageGuids.ts4WinuiPackageCmdSet,
                 AddTesting,
                 RightClickAvailable,
                 TemplateType.Testing);

            openTempFolderCommand = new TemplateStudioCommand(
                this,
                 commandService,
                PackageIds.OpenTempFolder,
                PackageGuids.ts4WinuiPackageCmdSet,
                OpenTempFolder,
                TempFolderAvailable,
                TemplateType.Unspecified);
        }

        private void AddPage(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (RightClickActions.VisibleForWinUI(TemplateType.Page))
            {
                RightClickActions.AddNewPage();
            }
        }

        private void AddFeature(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (RightClickActions.VisibleForWinUI(TemplateType.Feature))
            {
                RightClickActions.AddNewFeature();
            }
        }

        private void AddService(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (RightClickActions.VisibleForWinUI(TemplateType.Service))
            {
                RightClickActions.AddNewService();
            }
        }

        private void AddTesting(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (RightClickActions.VisibleForWinUI(TemplateType.Testing))
            {
                RightClickActions.AddNewTesting();
            }
        }

        private void OpenTempFolder(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (RightClickActions.TempFolderAvailable() && RightClickActions.VisibleForWinUI())
            {
                RightClickActions.OpenTempFolder();
            }
        }

        private void RightClickAvailable(object sender, TemplateType templateType)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var cmd = (OleMenuCommand)sender;
            cmd.Enabled = RightClickActions.Enabled();
            cmd.Visible = RightClickActions.VisibleForWinUI(templateType);
        }

        private void TempFolderAvailable(object sender, TemplateType templateType)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var cmd = (OleMenuCommand)sender;
            cmd.Visible = RightClickActions.VisibleForWinUI()
                && RightClickActions.TempFolderAvailable();
        }
    }
}
