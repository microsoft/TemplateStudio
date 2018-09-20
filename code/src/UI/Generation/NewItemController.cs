// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

using Microsoft.TemplateEngine.Edge.Template;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.PostActions;
using Microsoft.Templates.Core.PostActions.Catalog.Merge;
using Microsoft.Templates.UI.Resources;
using Microsoft.Templates.UI.Services;
using Microsoft.Templates.UI.ViewModels.Common;
using Microsoft.VisualStudio.TemplateWizard;
using Newtonsoft.Json;

namespace Microsoft.Templates.UI
{
    public class NewItemController
    {
        private static Lazy<NewItemController> _instance = new Lazy<NewItemController>(Initialize);

        public static NewItemController Instance => _instance.Value;

        private static NewItemController Initialize()
        {
            return new NewItemController();
        }

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
                ShowError(ex);
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
                ShowError(ex);
            }

            GenContext.ToolBox.Shell.CancelWizard();

            return null;
        }

        public async Task GenerateNewItemAsync(TemplateType templateType, UserSelection userSelection)
        {
            try
            {
               await NewItemGenController.Instance.UnsafeGenerateNewItemAsync(templateType, userSelection);
            }
            catch (Exception ex)
            {
                ShowError(ex, userSelection);

                GenContext.ToolBox.Shell.CancelWizard(false);
            }
        }

        public void FinishGeneration(UserSelection userSelection)
        {
            try
            {
                NewItemGenController.Instance.UnsafeFinishGeneration(userSelection);
            }
            catch (Exception ex)
            {
                ShowError(ex, userSelection);
                GenContext.ToolBox.Shell.CancelWizard(false);
            }
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

        internal void ShowError(Exception ex, UserSelection userSelection = null)
        {
            AppHealth.Current.Error.TrackAsync(ex.ToString()).FireAndForget();
            AppHealth.Current.Exception.TrackAsync(ex, userSelection?.ToString()).FireAndForget();

            var vm = new ErrorDialogViewModel(ex);
            var error = new Views.Common.ErrorDialog(vm);

            GenContext.ToolBox.Shell.ShowModal(error);
        }

        internal void CleanStatusBar()
        {
            GenContext.ToolBox.Shell.ShowStatusBarMessage(string.Empty);
        }
    }
}
