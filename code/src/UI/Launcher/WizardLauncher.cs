// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Helpers;
using Microsoft.Templates.Core.Naming;
using Microsoft.Templates.Core.Resources;
using Microsoft.Templates.UI.Services;
using Microsoft.Templates.UI.Views;
using Microsoft.VisualStudio.TemplateWizard;
using Newtonsoft.Json;

namespace Microsoft.Templates.UI.Launcher
{
    public class WizardLauncher
    {
        private DialogService _dialogService = DialogService.Instance;
        private static Lazy<WizardLauncher> _instance = new Lazy<WizardLauncher>(() => new WizardLauncher());

        public static WizardLauncher Instance => _instance.Value;

        private WizardLauncher()
        {
        }

        public UserSelection StartNewProject(string platform, string language, BaseStyleValuesProvider provider)
        {
            var validationService = new ProjectNameService(GenContext.ToolBox.Repo.ProjectNameValidationConfig, () => new List<string>());
            var projectName = GenContext.Current.ProjectName;
            var projectNameValidation = validationService.Validate(projectName);

            if (projectNameValidation.IsValid)
            {
                var newProjectView = new Views.NewProject.WizardShell(platform, language, provider);
                return StartWizard(newProjectView, WizardTypeEnum.NewProject);
            }
            else
            {
                var message = string.Empty;
                switch (projectNameValidation.ErrorType)
                {
                    case ValidationErrorType.ReservedName:
                        message = string.Format(StringRes.ErrorProjectReservedName, projectName);
                        break;
                    case ValidationErrorType.Regex:
                        message = string.Format(StringRes.ErrorProjectStartsWith, projectName, projectName[0]);
                        break;
                }

                var title = StringRes.ErrorTitleInvalidProjectName;
                MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
                CancelWizard();
                return null;
            }
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
    }
}
