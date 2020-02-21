// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Naming;

using Microsoft.Templates.UI.Services;
using Microsoft.Templates.UI.ViewModels.Common;
using Microsoft.Templates.UI.Views;
using Microsoft.Templates.UI.VisualStudio;
using Microsoft.VisualStudio.TemplateWizard;
using CoreStringRes = Microsoft.Templates.Core.Resources.StringRes;
using UIStringRes = Microsoft.Templates.UI.Resources.StringRes;
using Microsoft.Templates.UI.Extensions;

namespace Microsoft.Templates.UI.Launcher
{
    public class WizardLauncher
    {
        private DialogService _dialogService = DialogService.Instance;
        private BaseStyleValuesProvider _styleProvider;
        private static Lazy<WizardLauncher> _instance = new Lazy<WizardLauncher>(() => new WizardLauncher());

        public static WizardLauncher Instance => _instance.Value;

        private WizardLauncher()
        {
        }

        public UserSelection StartNewProject(string platform, string language, string requiredWorkload, BaseStyleValuesProvider provider)
        {
            _styleProvider = provider;

            CheckVSVersion(platform);
            CheckForMissingWorkloads(platform, requiredWorkload);
            CheckForInvalidProjectName();

            var newProjectView = new Views.NewProject.WizardShell(platform, language, provider);
            return StartWizard(newProjectView, WizardTypeEnum.NewProject);
        }

        public UserSelection StartAddTemplate(string language, BaseStyleValuesProvider provider, TemplateType templateType, WizardTypeEnum wizardTypeEnum)
        {
            var addTemplateView = new Views.NewItem.WizardShell(templateType, language, provider);
            return StartWizard(addTemplateView, wizardTypeEnum);
        }

        private UserSelection StartWizard(IWizardShell wizardShell, WizardTypeEnum wizardType)
        {
            try
            {
                CleanStatusBar();
                var userSelection = LaunchWizardShell(wizardShell);
                SendTelemetry(wizardType, userSelection);

                if (userSelection != null)
                {
                    return userSelection;
                }
            }
            catch (Exception ex) when (!(ex is WizardBackoutException))
            {
                (wizardShell as IWindow).SafeClose();
                _dialogService.ShowError(ex);
            }

            CancelWizard();
            return null;
        }

        private void CleanStatusBar() => GenContext.ToolBox.Shell.ShowStatusBarMessage(string.Empty);

        private void CancelWizard() => GenContext.ToolBox.Shell.CancelWizard();

        private UserSelection LaunchWizardShell(IWizardShell wizardShell)
        {
            GenContext.ToolBox.Shell.ShowModal(wizardShell as IWindow);
            return wizardShell.Result;
        }

        private void SendTelemetry(WizardTypeEnum wizardType, UserSelection userSelection)
        {
            if (userSelection is null)
            {
                SendTelemetryWizardCanceled(wizardType);
                return;
            }

            WizardActionEnum wizardAction;
            switch (userSelection.ItemGenerationType)
            {
                case ItemGenerationType.Generate:
                    wizardAction = WizardActionEnum.GenerateItem;
                    break;
                case ItemGenerationType.GenerateAndMerge:
                    wizardAction = WizardActionEnum.GenerateAndMergeItem;
                    break;
                default:
                    wizardAction = WizardActionEnum.GenerateProject;
                    break;
            }

            SendTelemetryWizardComplete(wizardType, wizardAction);
        }

        private void SendTelemetryWizardComplete(WizardTypeEnum wizardType, WizardActionEnum wizardAction)
        {
            AppHealth.Current.Telemetry.TrackWizardCompletedAsync(
                wizardType,
                wizardAction,
                GenContext.ToolBox.Shell.GetVsVersion())
                .FireAndForget();
        }

        private void SendTelemetryWizardCanceled(WizardTypeEnum wizardType)
        {
            AppHealth.Current.Telemetry.TrackWizardCancelledAsync(
                wizardType,
                GenContext.ToolBox.Shell.GetVsVersion(),
                GenContext.ToolBox.Repo.SyncInProgress)
                .FireAndForget();
        }

        private void CheckVSVersion(string platform)
        {
            var vsInfo = GenContext.ToolBox.Shell.GetVSTelemetryInfo();
            if (!string.IsNullOrEmpty(vsInfo.VisualStudioExeVersion))
            {
                // VisualStudioExeVersion is Empty on UI Test or VSEmulator execution
                var version = Version.Parse(vsInfo.VisualStudioExeVersion);
                if (platform == Platforms.Wpf && (version.Major < 16 || (version.Major == 16 && version.Minor < 3)))
                {
                    var title = UIStringRes.InfoDialogInvalidVersionTitle;
                    var message = UIStringRes.InfoDialogInvalidVSVersionForWPF;
                    var link = "https://docs.microsoft.com/en-us/visualstudio/install/install-visual-studio";

                    var vm = new InfoDialogViewModel(title, message, link, _styleProvider);
                    var info = new Views.Common.InfoDialog(vm);
                    GenContext.ToolBox.Shell.ShowModal(info);

                    CancelWizard();
                }
            }
        }

        private void CheckForMissingWorkloads(string platform, string requiredWorkload)
        {
            var vsShell = GenContext.ToolBox.Shell as VsGenShell;
            if (vsShell != null)
            {
                if (!vsShell.GetInstalledPackageIds().Contains(requiredWorkload))
                {
                    var title = UIStringRes.InfoDialogMissingWorkloadTitle;
                    var message = string.Format(UIStringRes.InfoDialogRequiredWorkloadNotFoundMessage, requiredWorkload.GetRequiredWorkloadDisplayName(), platform.GetPlatformDisplayName());
                    var link = "https://docs.microsoft.com/en-us/visualstudio/install/install-visual-studio";

                    var vm = new InfoDialogViewModel(title, message, link, _styleProvider);
                    var info = new Views.Common.InfoDialog(vm);
                    GenContext.ToolBox.Shell.ShowModal(info);

                    CancelWizard();
                }
            }
        }

        private void CheckForInvalidProjectName()
        {
            var validationService = new ProjectNameService(GenContext.ToolBox.Repo.ProjectNameValidationConfig, () => new List<string>());
            var projectName = GenContext.Current.ProjectName;
            var projectNameValidation = validationService.Validate(projectName);

            if (!projectNameValidation.IsValid)
            {
                var message = string.Empty;
                switch (projectNameValidation.Errors.FirstOrDefault()?.ErrorType)
                {
                    case ValidationErrorType.ReservedName:
                        message = string.Format(CoreStringRes.ErrorProjectReservedName, projectName);
                        break;
                    case ValidationErrorType.Regex:
                        message = string.Format(CoreStringRes.ErrorProjectStartsWith, projectName, projectName[0]);
                        break;
                }

                var title = CoreStringRes.ErrorTitleInvalidProjectName;
                var link = "https://github.com/microsoft/WindowsTemplateStudio/blob/master/docs/WTSNaming.md";
                var vm = new InfoDialogViewModel(title, message, link, _styleProvider);
                var info = new Views.Common.InfoDialog(vm);
                GenContext.ToolBox.Shell.ShowModal(info);

                CancelWizard();
            }
        }
    }
}
