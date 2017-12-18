// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.V2Services;
using Microsoft.Templates.UI.V2ViewModels.Common;
using Microsoft.Templates.UI.V2Views.NewProject;

namespace Microsoft.Templates.UI.V2ViewModels.NewProject
{
    public class MainViewModel : BaseMainViewModel
    {
        private static MainViewModel _instance;

        public static MainViewModel Instance => _instance ?? (_instance = new MainViewModel());

        private int _step;

        public int Step
        {
            get => _step;
            set => SetProperty(ref _step, value);
        }

        public ProjectTypeViewModel ProjectType { get; } = new ProjectTypeViewModel();

        public FrameworkViewModel Framework { get; } = new FrameworkViewModel();

        public AddPagesViewModel AddPages { get; } = new AddPagesViewModel();

        public AddFeaturesViewModel AddFeatures { get; } = new AddFeaturesViewModel();

        public TemplateSelectionViewModel TemplateSelection { get; } = new TemplateSelectionViewModel();

        private MainViewModel()
        {
            EventService.Instance.OnProjectTypeChanged += OnProjectTypeSelectionChanged;
            EventService.Instance.OnFrameworkChanged += OnFrameworkSelectionChanged;
            EventService.Instance.OnTemplateClicked += OnTemplateClicked;
            ValidationService.Initialize(TemplateSelection.GetNames);
        }

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();
        }

        protected override void OnCancel()
        {
            WizardShell.Current.Close();
        }

        protected override void OnGoBack()
        {
            switch (Step)
            {
                case 1: // GoBack button Clicked on FrameworkPage
                    NavigationService.GoBackMainFrame();
                    Step--;
                    SetCanGoBack(false);
                    break;
                case 2: // GoBack button Clicked on PagesPage
                    NavigationService.GoBackMainFrame();
                    Step--;
                    break;
                case 3: // GoBack button Clicked on FeaturesPage
                    NavigationService.GoBackMainFrame();
                    Step--;
                    SetCanGoForward(true);
                    break;
            }
        }

        protected override void OnGoForward()
        {
            switch (Step)
            {
                case 0: // GoForward button Clicked on ProjectTypePage
                    NavigationService.NavigateSecondaryFrame(new FrameworkPage());
                    Step++;
                    SetCanGoBack(true);
                    break;
                case 1: // GoForward button Clicked on FrameworkPage
                    NavigationService.NavigateSecondaryFrame(new AddPagesPage());
                    Step++;
                    break;
                case 2: // GoForward button Clicked on AddPagesPage
                    NavigationService.NavigateSecondaryFrame(new AddFeaturesPage());
                    Step++;
                    SetCanGoForward(false);
                    break;
            }
        }

        protected override void OnFinish()
        {
        }

        private void OnProjectTypeSelectionChanged(object sender, MetadataInfoViewModel projectType)
        {
            Framework.LoadData(projectType.Name);
        }

        private void OnFrameworkSelectionChanged(object sender, MetadataInfoViewModel framework)
        {
            AddPages.LoadData(framework.Name);
            AddFeatures.LoadData(framework.Name);
        }

        private void OnTemplateClicked(object sender, TemplateInfoViewModel selectedTemplate)
        {
            var newTemplate = TemplateSelection.Add(selectedTemplate);
            if (newTemplate != null)
            {
                // Update count on TemplateInfoViewModel
                var groups = (selectedTemplate.TemplateType == TemplateType.Page) ? AddPages.Groups : AddFeatures.Groups;
                foreach (var group in groups)
                {
                    var template = group.Items.FirstOrDefault(t => t == selectedTemplate);
                    if (template != null)
                    {
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
