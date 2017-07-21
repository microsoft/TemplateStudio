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

        public ProjectSetupViewModel ProjectSetup { get; private set; } = new ProjectSetupViewModel();

        public ProjectTemplatesViewModel ProjectTemplates { get; private set; } = new ProjectTemplatesViewModel();

        public ObservableCollection<SummaryLicenseViewModel> SummaryLicenses { get; } = new ObservableCollection<SummaryLicenseViewModel>();

        private bool _hasSummaryLicenses;
        public bool HasSummaryLicenses
        {
            get => _hasSummaryLicenses;
            private set => SetProperty(ref _hasSummaryLicenses, value);
        }

        public MainViewModel(MainView mainView, string language) : base(mainView)
        {
            MainView = mainView;
            Current = this;
        }

        private StackPanel _summaryPageGroups;
        public async Task InitializeAsync(Frame stepFrame, StackPanel summaryPageGroups)
        {
            NavigationService.Initialize(stepFrame, new ProjectSetupView());
            _summaryPageGroups = summaryPageGroups;
            await BaseInitializeAsync();
            SummaryLicenses.CollectionChanged += (s, o) => { OnPropertyChanged(nameof(SummaryLicenses)); };
        }

        public void AlertProjectSetupChanged()
        {
            if (CheckProjectSetupChanged())
            {
                SetStatus(StatusViewModel.Warning(string.Format(StringRes.ResetSelection, ProjectTemplates.ContextProjectType.DisplayName, ProjectTemplates.ContextFramework.DisplayName)));
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
            if (button != null && button.Tag != null && button.Tag.ToString() == "AllowCloseEdition")
            {
                return;
            }

            var templateInfo = textBox.Tag as TemplateInfoViewModel;
            if (templateInfo != null)
            {
                templateInfo.CloseEdition();
            }

            var summaryItem = textBox.Tag as SavedTemplateViewModel;
            if (summaryItem != null)
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

        protected override void OnNext()
        {
            base.OnNext();
            if (CheckProjectSetupChanged())
            {
                ProjectTemplates.ResetSelection();
                _summaryPageGroups.Children.Clear();
                CleanStatus();
            }
            NavigationService.Navigate(new ProjectTemplatesView());
        }
        protected override void OnFinish(string parameter)
        {
            MainView.Result = CreateUserSelection();
            base.OnFinish(parameter);
        }
        protected override async void OnTemplatesAvailable() => await ProjectSetup.InitializeAsync();
        protected override async void OnNewTemplatesAvailable()
        {
            UpdateCanFinish(false);
            _canGoBack = false;
            BackCommand.OnCanExecuteChanged();
            ShowFinishButton = false;
            EnableGoForward();
            ProjectTemplates.ResetSelection();
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
            if (ProjectTemplates.HasTemplatesAdded && (FrameworkChanged || ProjectTypeChanged))
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
    }
}
