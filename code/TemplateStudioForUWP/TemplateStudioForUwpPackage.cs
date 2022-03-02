using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Templates.Core;
using Microsoft.Templates.UI.VisualStudio;
using Microsoft.VisualStudio.Shell;
using TemplateStudioForUwp.Commands;
using Task = System.Threading.Tasks.Task;

namespace TemplateStudioForUwp
{
    [ProvideAutoLoad(ActivationContextGuid, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideUIContextRule(ActivationContextGuid,
        name: "Load TW4UWP Project Package",
        expression: "HasUWP",
        termNames: new[] { "HasUWP" },
        termValues: new[] { "SolutionHasProjectFlavor:{A5A43C5B-DE2A-4C0C-9213-0A381AF9435A}" })]
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
            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            await InitializeCommandsAsync();

            await base.InitializeAsync(cancellationToken, progress);
        }

        private async Task InitializeCommandsAsync()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(this.DisposalToken);

            OleMenuCommandService commandService = await this.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;

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
            cmd.Enabled = RightClickActions.Enabled();
            cmd.Visible = RightClickActions.VisibleForUwp(templateType);
        }

        private void TempFolderAvailable(object sender, TemplateType templateType)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var cmd = (OleMenuCommand)sender;
            cmd.Visible = RightClickActions.VisibleForUwp() && RightClickActions.TempFolderAvailable();
        }
    }
}
