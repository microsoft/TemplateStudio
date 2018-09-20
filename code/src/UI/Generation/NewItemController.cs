// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.UI.Services;
using Microsoft.Templates.UI.ViewModels.Common;
using Microsoft.VisualStudio.TemplateWizard;
using System;
using System.Threading.Tasks;

namespace Microsoft.Templates.UI
{
    public class NewItemController
    {
        private DialogService _dialogService = DialogService.Instance;
        private static Lazy<NewItemController> _instance = new Lazy<NewItemController>(() => new NewItemController());

        public static NewItemController Instance => _instance.Value;

        private NewItemController()
        {
        }

        public UserSelection GetUserSelectionNewFeature(string language, BaseStyleValuesProvider provider)
        {
            var newItem = new Views.NewItem.WizardShell(TemplateType.Feature, language, provider);
            try
            {
                CleanStatusBar();

                GenContext.ToolBox.Shell.ShowModal(newItem);
                if (newItem.Result != null)
                {
                    TrackWizardCompletedTelemery(WizardTypeEnum.AddFeature, newItem.Result.ItemGenerationType);

                    return newItem.Result;
                }
                else
                {
                    AppHealth.Current.Telemetry.TrackWizardCancelledAsync(WizardTypeEnum.AddFeature, GenContext.ToolBox.Shell.GetVsVersion(), GenContext.ToolBox.Repo.SyncInProgress).FireAndForget();
                }
            }
            catch (Exception ex) when (!(ex is WizardBackoutException))
            {
                newItem.SafeClose();
                _dialogService.ShowError(ex);
            }

            GenContext.ToolBox.Shell.CancelWizard();

            return null;
        }

        public UserSelection GetUserSelectionNewPage(string language, BaseStyleValuesProvider provider)
        {
            var newItem = new Views.NewItem.WizardShell(TemplateType.Page, language, provider);
            try
            {
                CleanStatusBar();

                GenContext.ToolBox.Shell.ShowModal(newItem);
                if (newItem.Result != null)
                {
                    TrackWizardCompletedTelemery(WizardTypeEnum.AddPage, newItem.Result.ItemGenerationType);

                    return newItem.Result;
                }
                else
                {
                    AppHealth.Current.Telemetry.TrackWizardCancelledAsync(WizardTypeEnum.AddPage, GenContext.ToolBox.Shell.GetVsVersion(), GenContext.ToolBox.Repo.SyncInProgress).FireAndForget();
                }
            }
            catch (Exception ex) when (!(ex is WizardBackoutException))
            {
                newItem.SafeClose();
                _dialogService.ShowError(ex);
            }

            GenContext.ToolBox.Shell.CancelWizard();

            return null;
        }

        private static void TrackWizardCompletedTelemery(WizardTypeEnum wizardType, ItemGenerationType generationType)
        {
            switch (generationType)
            {
                case ItemGenerationType.Generate:
                    AppHealth.Current.Telemetry.TrackWizardCompletedAsync(wizardType, WizardActionEnum.GenerateItem, GenContext.ToolBox.Shell.GetVsVersion()).FireAndForget();

                    break;
                case ItemGenerationType.GenerateAndMerge:
                    AppHealth.Current.Telemetry.TrackWizardCompletedAsync(wizardType, WizardActionEnum.GenerateAndMergeItem, GenContext.ToolBox.Shell.GetVsVersion()).FireAndForget();
                    break;
                default:
                    break;
            }
        }

        internal void CleanStatusBar() => GenContext.ToolBox.Shell.ShowStatusBarMessage(string.Empty);
    }
}
