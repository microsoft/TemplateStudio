// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.UI.V2Controls;
using Microsoft.Templates.UI.V2Resources;
using Microsoft.Templates.UI.V2Services;
using Microsoft.Templates.UI.V2ViewModels.Common;
using Microsoft.Templates.UI.V2Views.NewProject;

namespace Microsoft.Templates.UI.V2ViewModels.NewProject
{
    public class MainViewModel : BaseMainViewModel
    {
        private static MainViewModel _instance;
        private TemplateInfoViewModel _selectedTemplate;

        public static MainViewModel Instance => _instance ?? (_instance = new MainViewModel(WizardShell.Current));

        public ProjectTypeViewModel ProjectType { get; } = new ProjectTypeViewModel(IsSelectionEnabled);

        public FrameworkViewModel Framework { get; } = new FrameworkViewModel(IsSelectionEnabled);

        public AddPagesViewModel AddPages { get; } = new AddPagesViewModel();

        public AddFeaturesViewModel AddFeatures { get; } = new AddFeaturesViewModel();

        public UserSelectionViewModel UserSelection { get; } = new UserSelectionViewModel();

        private MainViewModel(Window mainView)
            : base(mainView)
        {
            ValidationService.Initialize(UserSelection.GetNames);
        }

        public override async Task InitializeAsync(string language)
        {
            await base.InitializeAsync(language);
        }

        protected override void OnCancel() => WizardShell.Current.Close();

        protected override void UpdateStep()
        {
            base.UpdateStep();
            Page destinationPage = null;
            switch (Step)
            {
                case 0:
                    destinationPage = new ProjectTypePage();
                    break;
                case 1:
                    destinationPage = new FrameworkPage();
                    break;
                case 2:
                    destinationPage = new AddPagesPage();
                    break;
                case 3:
                    destinationPage = new AddFeaturesPage();
                    break;
            }

            if (destinationPage != null)
            {
                NavigationService.NavigateSecondaryFrame(destinationPage);
                SetCanGoBack(Step > 0);
                SetCanGoForward(Step < 3);
            }
        }

        protected override void OnFinish()
        {
            WizardShell.Current.Result = UserSelection.GetUserSelection();
            base.OnFinish();
        }

        private static bool IsSelectionEnabled()
        {
            bool result = false;
            if (!Instance.UserSelection.HasItemsAddedByUser)
            {
                result = true;
            }
            else
            {
                var messageResult = MessageBox.Show("Do you want to restart the selection?", "Project configuration", MessageBoxButton.YesNo);
                if (messageResult == MessageBoxResult.Yes)
                {
                    Instance.UserSelection.ResetUserSelection();
                    result = true;
                }
                else
                {
                    result = false;
                }
            }

            if (result == true)
            {
                Instance.AddPages.ResetUserSelection();
                Instance.AddFeatures.ResetTemplatesCount();
            }

            return result;
        }

        public TemplateInfoViewModel GetTemplate(ITemplateInfo templateInfo)
        {
            var groups = templateInfo.GetTemplateType() == TemplateType.Page ? AddPages.Groups : AddFeatures.Groups;
            foreach (var group in groups)
            {
                var template = group.GetTemplate(templateInfo);
                if (template != null)
                {
                    return template;
                }
            }

            return null;
        }

        private void AddTemplate(TemplateInfoViewModel selectedTemplate)
        {
            if (selectedTemplate.MultipleInstance || !UserSelection.IsTemplateAdded(selectedTemplate))
            {
                UserSelection.Add(TemplateOrigin.UserSelection, selectedTemplate);
            }
        }

        protected override Task OnTemplatesAvailableAsync()
        {
            ProjectType.LoadData();
            return Task.CompletedTask;
        }

        protected override IEnumerable<Step> GetSteps()
        {
            yield return new Step(0, StringRes.NewProjectStepOne, true);
            yield return new Step(1, StringRes.NewProjectStepTwo);
            yield return new Step(2, StringRes.NewProjectStepThree);
            yield return new Step(3, StringRes.NewProjectStepFour);
        }

        public override void ProcessItem(object item)
        {
            if (item is MetadataInfoViewModel metadata)
            {
                if (metadata.IsProjectType())
                {
                    ProjectType.Selected = metadata;
                    Framework.LoadData(metadata.Name);
                }
                else if (metadata.IsFramework())
                {
                    Framework.Selected = metadata;
                    AddPages.LoadData(metadata.Name);
                    AddFeatures.LoadData(metadata.Name);
                    UserSelection.Initialize(ProjectType.Selected.Name, Framework.Selected.Name, Language);
                    WizardStatus.IsLoading = false;
                }
            }
            else if (item is TemplateInfoViewModel template)
            {
                _selectedTemplate = template;
                AddTemplate(template);
            }
        }
    }
}
