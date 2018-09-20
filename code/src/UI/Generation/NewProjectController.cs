// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.TemplateEngine.Edge.Template;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.PostActions;
using Microsoft.Templates.UI.Resources;
using Microsoft.Templates.UI.Services;
using Microsoft.Templates.UI.ViewModels.Common;
using Microsoft.VisualStudio.TemplateWizard;

namespace Microsoft.Templates.UI
{
    public class NewProjectController
    {
        private static Lazy<NewProjectController> _instance = new Lazy<NewProjectController>(Initialize);

        public static NewProjectController Instance => _instance.Value;

        private static NewProjectController Initialize()
        {
            return new NewProjectController();
        }

        private NewProjectController()
        {
        }

        public UserSelection GetUserSelection(string platform, string language, BaseStyleValuesProvider provider)
        {
            var mainView = new Views.NewProject.WizardShell(platform, language, provider);

            try
            {
                CleanStatusBar();

                GenContext.ToolBox.Shell.ShowModal(mainView);
                if (mainView.Result != null)
                {
                    AppHealth.Current.Telemetry.TrackWizardCompletedAsync(WizardTypeEnum.NewProject, WizardActionEnum.GenerateProject, GenContext.ToolBox.Shell.GetVsVersion()).FireAndForget();

                    return mainView.Result;
                }
                else
                {
                    AppHealth.Current.Telemetry.TrackWizardCancelledAsync(WizardTypeEnum.NewProject, GenContext.ToolBox.Shell.GetVsVersion(), GenContext.ToolBox.Repo.SyncInProgress).FireAndForget();
                }
            }
            catch (Exception ex) when (!(ex is WizardBackoutException))
            {
                mainView.SafeClose();
                ShowError(ex);
            }

            GenContext.ToolBox.Shell.CancelWizard();

            return null;
        }

        public async Task GenerateProjectAsync(UserSelection userSelection)
        {
            try
            {
                await NewProjectGenController.Instance.UnsafeGenerateProjectAsync(userSelection);
            }
            catch (Exception ex)
            {
                GenContext.ToolBox.Shell.CloseSolution();

                ShowError(ex, userSelection);

                GenContext.ToolBox.Shell.CancelWizard(false);
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
