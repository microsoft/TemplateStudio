// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;
using Microsoft.Templates.Core;
using Microsoft.Templates.UI.Generation;
using Microsoft.Templates.UI.V2Controls;
using Microsoft.Templates.UI.V2Resources;
using Microsoft.Templates.UI.V2Services;
using Microsoft.Templates.UI.V2ViewModels.Common;
using Microsoft.Templates.UI.V2Views.NewItem;

namespace Microsoft.Templates.UI.V2ViewModels.NewItem
{
    public class MainViewModel : BaseMainViewModel
    {
        private static MainViewModel _instance;
        private TemplateType _templateType;

        public string ConfigFramework { get; private set; }

        public string ConfigProjectType { get; private set; }

        public static MainViewModel Instance => _instance ?? (_instance = new MainViewModel(WizardShell.Current));

        public TemplateSelectionViewModel TemplateSelection { get; } = new TemplateSelectionViewModel();

        public MainViewModel(WizardShell mainWindow)
            : base(mainWindow)
        {
        }

        public async Task InitializeAsync(TemplateType templateType, string language)
        {
            _templateType = templateType;

            var stringResource = templateType == TemplateType.Page ? StringRes.NewItemTitlePage : StringRes.NewItemTitleFeature;
            WizardStatus.Title = stringResource;
            await InitializeAsync(language);
        }

        protected override void UpdateStep()
        {
            base.UpdateStep();
            Page destinationPage = null;
            switch (Step)
            {
                case 0:
                    destinationPage = new TemplateSelectionPage();
                    break;
                case 1:
                    destinationPage = new ChangesSummaryPage();
                    break;
            }
            if (destinationPage != null)
            {
                NavigationService.NavigateSecondaryFrame(destinationPage);
                SetCanGoBack(Step > 0);
                SetCanGoForward(Step < 1);
            }
        }

        protected override void OnCancel()
        {
        }

        protected override Task OnTemplatesAvailableAsync()
        {
            SetProjectConfigInfo();
            TemplateSelection.LoadData(_templateType, ConfigFramework);
            WizardStatus.IsLoading = false;
            return Task.CompletedTask;
        }

        private void SetProjectConfigInfo()
        {
            var configInfo = ProjectConfigInfo.ReadProjectConfiguration();
            if (string.IsNullOrEmpty(configInfo.ProjectType) || string.IsNullOrEmpty(configInfo.Framework))
            {
                // TODO: mvegaca
                //WizardStatus.InfoShapeVisibility = Visibility.Visible;
                //ProjectConfigurationWindow projectConfig = new ProjectConfigurationWindow(MainView);

                //if (projectConfig.ShowDialog().Value)
                //{
                //    configInfo.ProjectType = projectConfig.ViewModel.SelectedProjectType.Name;
                //    configInfo.Framework = projectConfig.ViewModel.SelectedFramework.Name;
                //    WizardStatus.InfoShapeVisibility = Visibility.Collapsed;
                //}
                //else
                //{
                //    Cancel();
                //}
            }

            ConfigFramework = configInfo.Framework;
            ConfigProjectType = configInfo.ProjectType;
        }

        protected override IEnumerable<Step> GetSteps()
        {
            yield return new Step(0, StringRes.NewItemStepOne, true);
            yield return new Step(1, StringRes.NewItemStepTwo);
        }
    }
}
