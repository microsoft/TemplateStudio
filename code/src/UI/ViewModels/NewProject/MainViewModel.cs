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
    public enum NewProjectStep
    {
        ProjectConfiguration = 0,
        AddPages = 1,
        AddTemplates = 2
    }

    public class MainViewModel : BaseMainViewModel
    {
        public static MainViewModel Current;
        public MainView MainView;

        public ProjectSetupViewModel ProjectSetup { get; private set; } = new ProjectSetupViewModel();

        public ProjectTemplatesViewModel ProjectTemplates { get; private set; } = new ProjectTemplatesViewModel();
        public ValidationsViewModel Validations { get; } = new ValidationsViewModel();

        public ObservableCollection<SummaryLicenseViewModel> SummaryLicenses { get; } = new ObservableCollection<SummaryLicenseViewModel>();

        private bool _hasSummaryLicenses;
        public bool HasSummaryLicenses
        {
            get => _hasSummaryLicenses;
            private set => SetProperty(ref _hasSummaryLicenses, value);
        }

        public MainViewModel(MainView mainView) : base(mainView)
        {
            MainView = mainView;
            Current = this;
        }

        private StackPanel _summaryPageGroups;
        public async Task InitializeAsync(Frame stepFrame, StackPanel summaryPageGroups)
        {
            WizardStatus.WizardTitle = StringRes.ProjectSetupTitle;
            NavigationService.Initialize(stepFrame, new ProjectSetupView());
            _summaryPageGroups = summaryPageGroups;
            await BaseInitializeAsync();
            SummaryLicenses.CollectionChanged += (s, o) => { OnPropertyChanged(nameof(SummaryLicenses)); };
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

        public void RebuildLicenses()
        {
            var userSelection = CreateUserSelection();
            var genItems = GenComposer.Compose(userSelection);

            var genLicenses = genItems
                                .SelectMany(s => s.Template.GetLicenses())
                                .Distinct(new TemplateLicenseEqualityComparer())
                                .ToList();

            SyncLicenses(genLicenses);
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
                        p.TryClose();
                    }
                }));

                ProjectTemplates?.SavedFeatures?.ToList()?.ForEach(f =>
                {
                    if (f.IsEditionEnabled)
                    {
                        f.ConfirmRenameCommand.Execute(f);
                        f.TryClose();
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
                if (CheckProjectSetupChanged())
                {
                    ResetSelection();
                    _summaryPageGroups.Children.Clear();
                    CleanStatus();
                }
                WizardStatus.WizardTitle = StringRes.ProjectPagesTitle;
                await ProjectTemplates.InitializeAsync();
                NavigationService.Navigate(new ProjectPagesView());
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
            _summaryPageGroups.Children.Clear();
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

        private void SyncLicenses(IEnumerable<TemplateLicense> licenses)
        {
            var toRemove = new List<SummaryLicenseViewModel>();

            foreach (var summaryLicense in SummaryLicenses)
            {
                if (!licenses.Any(l => l.Url == summaryLicense.Url))
                {
                    toRemove.Add(summaryLicense);
                }
            }

            foreach (var licenseToRemove in toRemove)
            {
                SummaryLicenses.Remove(licenseToRemove);
            }

            foreach (var license in licenses)
            {
                if (!SummaryLicenses.Any(l => l.Url == license.Url))
                {
                    SummaryLicenses.Add(new SummaryLicenseViewModel(license));
                }
            }

            HasSummaryLicenses = SummaryLicenses.Any();
        }
        private bool CheckProjectSetupChanged()
        {
            if (HasTemplatesAdded && (FrameworkChanged || ProjectTypeChanged))
            {
                return true;
            }
            return false;
        }
        private bool FrameworkChanged => ProjectTemplates.ContextFramework.Name != ProjectSetup.SelectedFramework.Name;
        private bool ProjectTypeChanged => ProjectTemplates.ContextProjectType.Name != ProjectSetup.SelectedProjectType.Name;

        public void DefineDragAndDrop(ObservableCollection<SavedTemplateViewModel> items, bool allowDragAndDrop)
        {
            var listView = new ListView()
            {
                ItemsSource = items,
                Style = MainView.FindResource("SummaryListViewStyle") as Style,
                Tag = "AllowRename",
                Focusable = false,
                ItemTemplate = MainView.FindResource("ProjectTemplatesSummaryItemTemplate") as DataTemplate
            };
            if (allowDragAndDrop)
            {
                var service = new DragAndDropService<SavedTemplateViewModel>(listView);
                service.ProcessDrop += ProjectTemplates.DropTemplate;
            }
            _summaryPageGroups.Children.Add(listView);
        }

        private SavedTemplateViewModel _currentDragginTemplate;
        private SavedTemplateViewModel _dropTargetTemplate;

        public void SavedTemplateGotFocus(SavedTemplateViewModel savedTemplate)
        {
            _dropTargetTemplate = savedTemplate;
        }

        public bool SavedTemplateSetDrag(SavedTemplateViewModel savedTemplate)
        {
            if (_currentDragginTemplate == null)
            {
                _currentDragginTemplate = savedTemplate;
                return true;
            }
            return false;
        }

        public bool SavedTemplateSetDrop(SavedTemplateViewModel savedTemplate)
        {
            if (_currentDragginTemplate != null && _dropTargetTemplate != null && _currentDragginTemplate.ItemName != _dropTargetTemplate.ItemName)
            {
                var newIndex = ProjectTemplates.SavedPages.First().IndexOf(_dropTargetTemplate);
                var oldIndex = ProjectTemplates.SavedPages.First().IndexOf(_currentDragginTemplate);
                ProjectTemplates.DropTemplate(null, new DragAndDropEventArgs<SavedTemplateViewModel>(null, _dropTargetTemplate, oldIndex, newIndex));
                _currentDragginTemplate = null;
                _dropTargetTemplate = null;
            }
            return false;
        }

        public bool ClearCurrentDragginTemplate()
        {
            if (_currentDragginTemplate != null)
            {
                _currentDragginTemplate = null;
                _dropTargetTemplate = null;
                return true;
            }
            return false;
        }

        public bool HasTemplatesAdded => ProjectTemplates.SavedPages.Any() || ProjectTemplates.SavedFeatures.Any();

        public void ResetSelection()
        {
            ProjectTemplates.SavedPages.Clear();
            ProjectTemplates.SavedFeatures.Clear();
            ProjectTemplates.PagesGroups.Clear();
            ProjectTemplates.FeatureGroups.Clear();
        }
    }
}
