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
        private DialogService _dialogService = DialogService.Instance;
        private static Lazy<NewProjectController> _instance = new Lazy<NewProjectController>(() => new NewProjectController());

        public static NewProjectController Instance => _instance.Value;

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
                _dialogService.ShowError(ex);
            }

            GenContext.ToolBox.Shell.CancelWizard();

            return null;
        }

        internal void CleanStatusBar() => GenContext.ToolBox.Shell.ShowStatusBarMessage(string.Empty);
    }
}
