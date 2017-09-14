// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using Microsoft.Templates.Core;
using Microsoft.Templates.UI.Controls;
using Microsoft.Templates.UI.Resources;
using Microsoft.Templates.UI.Services;
using Microsoft.Templates.UI.ViewModels.Common;
using Microsoft.Templates.UI.Views.NewProject;

namespace Microsoft.Templates.UI.ViewModels.NewProject
{
    public class MainViewModel : BaseMainViewModel
    {
        public static MainViewModel Current;
        public MainView MainView;

        public ProjectSetupViewModel ProjectSetup { get; } = new ProjectSetupViewModel();

        public ProjectTemplatesViewModel ProjectTemplates { get; } = new ProjectTemplatesViewModel();

        public LicensesService Licenses { get; }
        public OrderingService Ordering { get; private set; }

        public MainViewModel(MainView mainView) : base(mainView)
        {
            MainView = mainView;
            Licenses = new LicensesService(CreateUserSelection);
            Current = this;
        }

        public async Task InitializeAsync(Frame stepFrame, StackPanel summaryPageGroups)
        {
            WizardStatus.WizardTitle = StringRes.ProjectSetupTitle;
            NavigationService.Initialize(stepFrame, new ProjectSetupView());
            Ordering = new OrderingService()
            {
                Panel = summaryPageGroups,
                ListViewStyle = MainView.FindResource("SummaryListViewStyle") as Style,
                ItemTemplate = MainView.FindResource("ProjectTemplatesSummaryItemTemplate") as DataTemplate,
                GetData = () => ProjectTemplates.SavedPages,
                SetHomePage = ProjectTemplates.SetHomePage
            };
            await BaseInitializeAsync();
        }

        public void AlertProjectSetupChanged()
        {
            if (CheckProjectSetupChanged())
            {
                WizardStatus.SetStatus(StatusViewModel.Warning(string.Format(StringRes.ResetSelection, ProjectTemplates.ContextProjectType.DisplayName, ProjectTemplates.ContextFramework.DisplayName)));
            }
            else
            {
                CleanStatus();
            }
        }

        public void TryCloseEdition(TextBoxEx textBox, Button button)
        {
            if (textBox == null)
            {
                return;
            }
            if (button?.Tag != null && button.Tag.ToString() == "AllowCloseEdition")
            {
                return;
            }

            if (textBox.Tag is TemplateInfoViewModel templateInfo)
            {
                templateInfo.CloseEdition();
            }

            if (textBox.Tag is SavedTemplateViewModel summaryItem)
            {
                ProjectTemplates.SavedPages.ToList().ForEach(spg => spg.ToList().ForEach(p =>
                {
                    if (p.IsEditionEnabled)
                    {
                        p.ConfirmRenameCommand.Execute(p);
                        p.Close();
                    }
                }));

                ProjectTemplates?.SavedFeatures?.ToList()?.ForEach(f =>
                {
                    if (f.IsEditionEnabled)
                    {
                        f.ConfirmRenameCommand.Execute(f);
                        f.Close();
                    }
                });
            }
        }

        protected override void OnCancel()
        {
            MainView.DialogResult = false;
            MainView.Result = null;
            MainView.Close();
        }

        protected override void OnClose()
        {
            MainView.DialogResult = true;
            MainView.Result = null;
            MainView.Close();
        }

        protected override async void OnNext()
        {
            base.OnNext();
            if (CurrentStep == 1)
            {
                WizardStatus.WizardTitle = StringRes.ProjectPagesTitle;
                await ProjectTemplates.InitializeAsync();
                NavigationService.Navigate(new ProjectPagesView());
                if (CheckProjectSetupChanged())
                {
                    ResetSelection();
                    Ordering.Panel.Children.Clear();
                    CleanStatus();
                }
            }
            else if (CurrentStep == 2)
            {
                WizardStatus.WizardTitle = StringRes.ProjectFeaturesTitle;
                NavigationService.Navigate(new ProjectFeaturesView());
                UpdateCanFinish(true);
            }
        }

        protected override void OnGoBack()
        {
            base.OnGoBack();
            if (CurrentStep == 0)
            {
                WizardStatus.WizardTitle = StringRes.ProjectSetupTitle;
            }
            else if (CurrentStep == 1)
            {
                WizardStatus.WizardTitle = StringRes.ProjectPagesTitle;
            }
        }

        protected override void OnFinish(string parameter)
        {
            if (CurrentStep == 2)
            {
                MainView.Result = CreateUserSelection();
                base.OnFinish(parameter);
            }
        }

        protected override async Task OnTemplatesAvailableAsync() => await ProjectSetup.InitializeAsync();

        protected override async Task OnNewTemplatesAvailableAsync()
        {
            ResetSelection();
            Ordering.Panel.Children.Clear();
            NavigationService.Navigate(new ProjectSetupView());
            await ProjectSetup.InitializeAsync(true);
        }

        public override UserSelection CreateUserSelection()
        {
            var userSelection = new UserSelection()
            {
                ProjectType = ProjectSetup.SelectedProjectType?.Name,
                Framework = ProjectSetup.SelectedFramework?.Name,
                HomeName = ProjectTemplates.HomeName
            };

            ProjectTemplates.SavedPages.ToList().ForEach(spg => userSelection.Pages.AddRange(spg.Select(sp => sp.UserSelection)));
            userSelection.Features.AddRange(ProjectTemplates.SavedFeatures.Select(sf => sf.UserSelection));

            return userSelection;
        }

        private bool CheckProjectSetupChanged()
        {
            bool hasTemplatesAdded = ProjectTemplates.SavedPages.Any() || ProjectTemplates.SavedFeatures.Any();
            bool frameworkChanged = ProjectTemplates.ContextFramework.Name != ProjectSetup.SelectedFramework.Name;
            var projectTypeChanged = ProjectTemplates.ContextProjectType.Name != ProjectSetup.SelectedProjectType.Name;

            return hasTemplatesAdded && (frameworkChanged || projectTypeChanged);
        }

        public void ResetSelection()
        {
            ProjectTemplates.SavedPages.Clear();
            ProjectTemplates.SavedFeatures.Clear();
            ProjectTemplates.PagesGroups.Clear();
            ProjectTemplates.FeatureGroups.Clear();
        }
    }
}
