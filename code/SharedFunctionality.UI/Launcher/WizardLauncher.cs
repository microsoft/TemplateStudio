﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Naming;
using Microsoft.Templates.Core.Services;
using Microsoft.Templates.SharedResources;
using Microsoft.Templates.UI.Extensions;
using Microsoft.Templates.UI.Services;
using Microsoft.Templates.UI.ViewModels.Common;
using Microsoft.Templates.UI.ViewModels.NewItem;
using Microsoft.Templates.UI.Views;
using Microsoft.Templates.UI.Views.Common;
using Microsoft.Templates.UI.Views.NewItem;
using Microsoft.Templates.UI.VisualStudio.GenShell;
using Microsoft.VisualStudio.TemplateWizard;

namespace Microsoft.Templates.UI.Launcher
{
    public class WizardLauncher
    {
        private readonly DialogService _dialogService = DialogService.Instance;
        private BaseStyleValuesProvider _styleProvider;
        private static readonly Lazy<WizardLauncher> _instance = new Lazy<WizardLauncher>(() => new WizardLauncher());

        public static WizardLauncher Instance => _instance.Value;

        private WizardLauncher()
        {
        }

        public UserSelection StartNewProject(UserSelectionContext context, string requiredVSVersion, string requiredWorkloads, BaseStyleValuesProvider provider)
        {
            _styleProvider = provider;

            CheckVSVersion(context.Platform, requiredVSVersion);
            CheckForMissingWorkloads(context.Platform, requiredWorkloads);
            CheckForInvalidProjectName();

            var newProjectView = new Views.NewProject.NewProjectWizardShell(context, provider);
            return StartWizard(newProjectView, WizardTypeEnum.NewProject);
        }

        public UserSelection StartAddTemplate(string language, BaseStyleValuesProvider provider, TemplateType templateType, WizardTypeEnum wizardTypeEnum)
        {
            var context = GetProjectConfigInfo(language);
            var addTemplateView = new Views.NewItem.NewItemWizardShell(templateType, context, provider);
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

        private static void CleanStatusBar() => GenContext.ToolBox.Shell.UI.ShowStatusBarMessage(string.Empty);

        private static void CancelWizard() => GenContext.ToolBox.Shell.UI.CancelWizard();

        private static UserSelection LaunchWizardShell(IWizardShell wizardShell)
        {
            GenContext.ToolBox.Shell.UI.ShowModal(wizardShell as IWindow);
            UserSelectionItem newBlank = new UserSelectionItem() { Name = "NewMain", TemplateId = "ts.WinUI.Page.Blank" };
            wizardShell.Result.Pages.Add(newBlank);
            return wizardShell.Result;
        }

        private static void SendTelemetry(WizardTypeEnum wizardType, UserSelection userSelection)
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

        private static void SendTelemetryWizardComplete(WizardTypeEnum wizardType, WizardActionEnum wizardAction)
        {
            AppHealth.Current.Telemetry.TrackWizardCompletedAsync(
                wizardType,
                wizardAction,
                GenContext.ToolBox.Shell.VisualStudio.GetVsVersion())
                .FireAndForget();
        }

        private static void SendTelemetryWizardCanceled(WizardTypeEnum wizardType)
        {
            AppHealth.Current.Telemetry.TrackWizardCancelledAsync(
                wizardType,
                GenContext.ToolBox.Shell.VisualStudio.GetVsVersion(),
                GenContext.ToolBox.Repo.SyncInProgress)
                .FireAndForget();
        }

        private void CheckVSVersion(string platform, string requiredVersion)
        {
            var vsInfo = GenContext.ToolBox.Shell.Telemetry.GetVSTelemetryInfo();
            if (!string.IsNullOrEmpty(requiredVersion) && !string.IsNullOrEmpty(vsInfo.VisualStudioExeVersion))
            {
                // VisualStudioExeVersion is Empty on UI Test or VSEmulator execution
                var actualVSVersion = Version.Parse(vsInfo.VisualStudioExeVersion);
                var requiredVSVersion = Version.Parse(requiredVersion);
                if (actualVSVersion.CompareTo(requiredVSVersion) < 0)
                {
                    var title = Resources.InfoDialogInvalidVersionTitle;
                    var message = string.Format(Resources.InfoDialogInvalidVSVersion, platform, requiredVersion);
                    var link = "https://docs.microsoft.com/en-us/visualstudio/install/install-visual-studio";

                    var vm = new InfoDialogViewModel(title, message, link, _styleProvider);
                    var info = new Views.Common.InfoDialog(vm);
                    GenContext.ToolBox.Shell.UI.ShowModal(info);

                    CancelWizard();
                }
            }
        }

        private void CheckForMissingWorkloads(string platform, string requiredWorkloads)
        {
            if (!string.IsNullOrEmpty(requiredWorkloads) && GenContext.ToolBox.Shell is VsGenShell vsShell)
            {
                var workloadsToCheck = requiredWorkloads.Split('|');
                var missingWorkloads = new List<string>();

                foreach (var workload in workloadsToCheck)
                {
                    if (!vsShell.VisualStudio.GetInstalledPackageIds().Contains(workload))
                    {
                        missingWorkloads.Add(workload.GetRequiredWorkloadDisplayName());
                    }
                }

                if (missingWorkloads.Count > 0)
                {
                    var title = Resources.InfoDialogMissingWorkloadTitle;
                    var message = string.Format(Resources.InfoDialogRequiredWorkloadNotFoundMessage, platform.GetPlatformDisplayName(), missingWorkloads.Aggregate((i, j) => $"{i}, {j}") );
                    var link = "https://docs.microsoft.com/en-us/visualstudio/install/install-visual-studio";

                    var vm = new InfoDialogViewModel(title, message, link, _styleProvider);
                    var info = new Views.Common.InfoDialog(vm);
                    GenContext.ToolBox.Shell.UI.ShowModal(info);

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
                        message = string.Format(Resources.ErrorProjectReservedName, projectName);
                        break;
                    case ValidationErrorType.Regex:
                        message = string.Format(Resources.ErrorProjectStartsWith, projectName, projectName[0]);
                        break;
                }

                var title = Resources.ErrorTitleInvalidProjectName;
                //// Keep this a WTS link as it's to docs
                var link = "https://github.com/microsoft/TemplateStudio/blob/main/docs/WTSNaming.md";
                var vm = new InfoDialogViewModel(title, message, link, _styleProvider);
                var info = new Views.Common.InfoDialog(vm);
                GenContext.ToolBox.Shell.UI.ShowModal(info);

                CancelWizard();
            }
        }

        private static UserSelectionContext GetProjectConfigInfo(string language)
        {
            var projectConfigInfoService = new ProjectConfigInfoService(GenContext.ToolBox.Shell);
            var configInfo = projectConfigInfoService.ReadProjectConfiguration();
            if (string.IsNullOrEmpty(configInfo.ProjectType) || string.IsNullOrEmpty(configInfo.Framework) || string.IsNullOrEmpty(configInfo.Platform))
            {
                var vm = new ProjectConfigurationViewModel(language);
                var projectConfig = new ProjectConfigurationDialog(vm);
                GenContext.ToolBox.Shell.UI.ShowModal(projectConfig);

                if (vm.Result == DialogResult.Accept)
                {
                    configInfo.ProjectType = vm.SelectedProjectType.Name;
                    configInfo.Framework = vm.SelectedFramework.Name;
                    configInfo.Platform = vm.SelectedPlatform;
                    if (configInfo.Platform == Platforms.WinUI)
                    {
                        configInfo.AppModel = vm.SelectedAppModel;
                    }

                    ProjectMetadataService.SaveProjectMetadata(configInfo, GenContext.ToolBox.Shell.Project.GetActiveProjectPath());
                    var userSeletion = new UserSelectionContext(language, configInfo.Platform)
                    {
                        ProjectType = configInfo.ProjectType,
                        FrontEndFramework = configInfo.Framework,
                    };

                    if (!string.IsNullOrEmpty(configInfo.AppModel))
                    {
                        userSeletion.AddAppModel(configInfo.AppModel);
                    }

                    return userSeletion;
                }

                return null;
            }
            else
            {
                var userSeletion = new UserSelectionContext(language, configInfo.Platform)
                {
                    ProjectType = configInfo.ProjectType,
                    FrontEndFramework = configInfo.Framework,
                };

                if (!string.IsNullOrEmpty(configInfo.AppModel))
                {
                    userSeletion.AddAppModel(configInfo.AppModel);
                }

                return userSeletion;
            }
        }
    }
}
