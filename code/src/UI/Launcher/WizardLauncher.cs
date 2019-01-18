// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.UI.Services;
using Microsoft.Templates.UI.Views;
using Microsoft.VisualStudio.TemplateWizard;

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
            var newProjectView = new Views.NewProject.WizardShell(platform, language, provider);
            return StartWizard(newProjectView, WizardTypeEnum.NewProject);
        }

        public UserSelection StartAddPage(string language, BaseStyleValuesProvider provider)
        {
            var addPageView = new Views.NewItem.WizardShell(TemplateType.Page, language, provider);
            return StartWizard(addPageView, WizardTypeEnum.AddPage);
        }

        public UserSelection StartAddFeature(string language, BaseStyleValuesProvider provider)
        {
            var addFeatureView = new Views.NewItem.WizardShell(TemplateType.Feature, language, provider);
            return StartWizard(addFeatureView, WizardTypeEnum.NewProject);
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
                wizardShell.SafeClose();
                _dialogService.ShowError(ex);
            }

            CancelWizard();
            return null;
        }

        private void CleanStatusBar() => GenContext.ToolBox.Shell.ShowStatusBarMessage(string.Empty);

        private void CancelWizard() => GenContext.ToolBox.Shell.CancelWizard();

        private UserSelection LaunchWizardShell(IWizardShell wizardShell)
        {
            GenContext.ToolBox.Shell.ShowModal(wizardShell as Window);
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
