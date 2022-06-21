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
using Microsoft.VisualStudio.Shell.Interop;
using TemplateStudioForUWP.Commands;
using Task = System.Threading.Tasks.Task;

// https://docs.microsoft.com/visualstudio/extensibility/how-to-use-asyncpackage-to-load-vspackages-in-the-background?view=vs-2022
// https://docs.microsoft.com/visualstudio/extensibility/how-to-use-rule-based-ui-context-for-visual-studio-extensions?view=vs-2022
// https://docs.microsoft.com/visualstudio/extensibility/internals/authoring-dot-vsct-files?view=vs-2022
// https://docs.microsoft.com/visualstudio/extensibility/command-flag-element?view=vs-2022

namespace TemplateStudioForUWP
{
    [ProvideAutoLoad(ActivationContextGuid, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideUIContextRule(ActivationContextGuid,
        name: "Load TW4UWP Project Package",
        expression: "HasUWP",
        termNames: new[] { "HasUWP" },
        termValues: new[] { "ActiveProjectOutputType:VSPROJ_OUTPUTTYPE_WINEXE" })]
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [Guid(PackageGuids.guidTemplateStudioForUwpPackageString)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    public sealed class TemplateStudioForUwpPackage : AsyncPackage
    {
        public const string ActivationContextGuid = "AD9D5551-71CA-4860-8071-2FDB57A89551";
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
            // When initialized asynchronously, the current thread may be a background thread at this point.
            // Do any initialization that requires the UI thread after switching to the UI thread.
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
                 PackageGuids.guidTemplateStudioForUwpPackageCmdSet,
                 AddPage,
                 RightClickAvailable,
                 TemplateType.Page);

            addFeatureCommand = new TemplateStudioCommand(
                this,
                 commandService,
                PackageIds.AddFeatureCommand,
                PackageGuids.guidTemplateStudioForUwpPackageCmdSet,
                AddFeature,
                RightClickAvailable,
                TemplateType.Feature);

            addServiceCommand = new TemplateStudioCommand(
                 this,
                 commandService,
                 PackageIds.AddServiceCommand,
                 PackageGuids.guidTemplateStudioForUwpPackageCmdSet,
                 AddService,
                 RightClickAvailable,
                 TemplateType.Service);

            addTestingCommand = new TemplateStudioCommand(
                 this,
                 commandService,
                 PackageIds.AddTestingCommand,
                 PackageGuids.guidTemplateStudioForUwpPackageCmdSet,
                 AddTesting,
                 RightClickAvailable,
                 TemplateType.Testing);

            openTempFolderCommand = new TemplateStudioCommand(
                this,
                 commandService,
                PackageIds.OpenTempFolder,
                PackageGuids.guidTemplateStudioForUwpPackageCmdSet,
                OpenTempFolder,
                TempFolderAvailable,
                TemplateType.Unspecified);
        }

        private void AddPage(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (RightClickActions.VisibleForUwp(TemplateType.Page))
            {
                RightClickActions.AddNewPage();
            }
        }

        private void AddFeature(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (RightClickActions.VisibleForUwp(TemplateType.Feature))
            {
                RightClickActions.AddNewFeature();
            }
        }

        private void AddService(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (RightClickActions.VisibleForUwp(TemplateType.Service))
            {
                RightClickActions.AddNewService();
            }
        }

        private void AddTesting(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (RightClickActions.VisibleForUwp(TemplateType.Testing))
            {
                RightClickActions.AddNewTesting();
            }
        }

        private void OpenTempFolder(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (RightClickActions.TempFolderAvailable() && RightClickActions.VisibleForUwp())
            {
                RightClickActions.OpenTempFolder();
            }
        }

        private void RightClickAvailable(object sender, TemplateType templateType)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var cmd = (OleMenuCommand)sender;
            cmd.Enabled = true; // RightClickActions.Enabled();
            cmd.Visible = true; // RightClickActions.VisibleForUwp(templateType);
        }

        private void TempFolderAvailable(object sender, TemplateType templateType)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var cmd = (OleMenuCommand)sender;
            cmd.Visible = RightClickActions.VisibleForUwp() && RightClickActions.TempFolderAvailable();
        }
    }
}
