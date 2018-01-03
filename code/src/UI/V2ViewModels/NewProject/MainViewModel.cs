// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Templates.Core;
using Microsoft.Templates.UI.V2Controls;
using Microsoft.Templates.UI.V2Services;
using Microsoft.Templates.UI.V2ViewModels.Common;
using Microsoft.Templates.UI.V2Views.NewProject;

namespace Microsoft.Templates.UI.V2ViewModels.NewProject
{
    public class MainViewModel : BaseMainViewModel
    {
        private static MainViewModel _instance;

        public static MainViewModel Instance => _instance ?? (_instance = new MainViewModel());

        public ProjectTypeViewModel ProjectType { get; } = new ProjectTypeViewModel(IsSelectionEnabled);

        public FrameworkViewModel Framework { get; } = new FrameworkViewModel(IsSelectionEnabled);

        public AddPagesViewModel AddPages { get; } = new AddPagesViewModel();

        public AddFeaturesViewModel AddFeatures { get; } = new AddFeaturesViewModel();

        public UserSelection UserSelection { get; } = new UserSelection();

        private MainViewModel()
        {
            EventService.Instance.OnProjectTypeChanged += OnProjectTypeSelectionChanged;
            EventService.Instance.OnFrameworkChanged += OnFrameworkSelectionChanged;
            EventService.Instance.OnTemplateClicked += OnTemplateClicked;
            ValidationService.Initialize(UserSelection.GetNames);
        }

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();
        }

        protected override void OnCancel()
        {
            WizardShell.Current.Close();
        }

        protected override void UpdateStep()
        {
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
                    Instance.AddPages.ResetUserSelection();
                    Instance.AddFeatures.ResetUserSelection();
                    result = true;
                }
                else
                {
                    result = false;
                }
            }

            return result;
        }

        private void OnProjectTypeSelectionChanged(object sender, MetadataInfoViewModel projectType)
        {
            Framework.LoadData(projectType.Name);
        }

        private void OnFrameworkSelectionChanged(object sender, MetadataInfoViewModel framework)
        {
            AddPages.LoadData(framework.Name);
            AddFeatures.LoadData(framework.Name);
            UserSelection.Initialize(ProjectType.Selected.Name, Framework.Selected.Name);
        }

        private void OnTemplateClicked(object sender, TemplateInfoViewModel selectedTemplate)
        {
            var newTemplate = UserSelection.Add(TemplateOrigin.UserSelection, selectedTemplate);
            if (newTemplate != null)
            {
                // Update count on TemplateInfoViewModel
                var groups = (selectedTemplate.TemplateType == TemplateType.Page) ? AddPages.Groups : AddFeatures.Groups;
                foreach (var group in groups)
                {
                    var template = group.Items.FirstOrDefault(t => t == selectedTemplate);
                    if (template != null)
                    {
                        newTemplate.UpdateSelection();
                        template.UpdateSelection(newTemplate);
                    }
                }
            }
        }

        protected override Task OnTemplatesAvailableAsync()
        {
            ProjectType.LoadData();
            return Task.CompletedTask;
        }
    }
}
