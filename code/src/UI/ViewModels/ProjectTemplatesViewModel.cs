// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.Resources;
using Microsoft.Templates.UI.Services;

namespace Microsoft.Templates.UI.ViewModels
{
    public class ProjectTemplatesViewModel : Observable
    {
        public MetadataInfoViewModel ContextFramework { get; set; }
        public MetadataInfoViewModel ContextProjectType { get; set; }

        private string _pagesHeader;
        public string PagesHeader
        {
            get => _pagesHeader;
            set => SetProperty(ref _pagesHeader, value);
        }

        private string _featuresHeader;
        public string FeaturesHeader
        {
            get => _featuresHeader;
            set => SetProperty(ref _featuresHeader, value);
        }

        public string HomeName { get; set; }

        public ObservableCollection<GroupTemplateInfoViewModel> PagesGroups { get; } = new ObservableCollection<GroupTemplateInfoViewModel>();
        public ObservableCollection<GroupTemplateInfoViewModel> FeatureGroups { get; } = new ObservableCollection<GroupTemplateInfoViewModel>();

        public ObservableCollection<ObservableCollection<SavedTemplateViewModel>> SavedPages { get; } = new ObservableCollection<ObservableCollection<SavedTemplateViewModel>>();
        public ObservableCollection<SavedTemplateViewModel> SavedFeatures { get; } = new ObservableCollection<SavedTemplateViewModel>();

        private RelayCommand<SavedTemplateViewModel> _removeTemplateCommand;
        public RelayCommand<SavedTemplateViewModel> RemoveTemplateCommand => _removeTemplateCommand ?? (_removeTemplateCommand = new RelayCommand<SavedTemplateViewModel>(OnRemoveTemplate));

        private RelayCommand<TemplateInfoViewModel> _addTemplateCommand;
        public RelayCommand<TemplateInfoViewModel> AddTemplateCommand => _addTemplateCommand ?? (_addTemplateCommand = new RelayCommand<TemplateInfoViewModel>(OnAddTemplateItem));

        private RelayCommand<TemplateInfoViewModel> _saveTemplateCommand;
        public RelayCommand<TemplateInfoViewModel> SaveTemplateCommand => _saveTemplateCommand ?? (_saveTemplateCommand = new RelayCommand<TemplateInfoViewModel>(OnSaveTemplateItem));

        private ICommand _openSummaryItemCommand;
        public ICommand OpenSummaryItemCommand => _openSummaryItemCommand ?? (_openSummaryItemCommand = new RelayCommand<SavedTemplateViewModel>(OnOpenSummaryItem));

        private ICommand _renameSummaryItemCommand;
        public ICommand RenameSummaryItemCommand => _renameSummaryItemCommand ?? (_renameSummaryItemCommand = new RelayCommand<SavedTemplateViewModel>(OnRenameSummaryItem));

        private ICommand _confirmRenameSummaryItemCommand;
        public ICommand ConfirmRenameSummaryItemCommand => _confirmRenameSummaryItemCommand ?? (_confirmRenameSummaryItemCommand = new RelayCommand<SavedTemplateViewModel>(OnConfirmRenameSummaryItem));

        public ProjectTemplatesViewModel()
        {
            SavedFeatures.CollectionChanged += (s, o) => { OnPropertyChanged(nameof(SavedFeatures)); };
            SavedPages.CollectionChanged += (s, o) => { OnPropertyChanged(nameof(SavedPages)); };
        }

        public IEnumerable<string> Names
        {
            get
            {
                var names = new List<string>();
                SavedPages.ToList().ForEach(spg => names.AddRange(spg.Select(sp => sp.ItemName)));
                names.AddRange(SavedFeatures.Select(sf => sf.ItemName));
                return names;
            }
        }

        public IEnumerable<string> Identities
        {
            get
            {
                var identities = new List<string>();
                SavedPages.ToList().ForEach(spg => identities.AddRange(spg.Select(sp => sp.Identity)));
                identities.AddRange(SavedFeatures.Select(sf => sf.Identity));
                return identities;
            }
        }

        public bool HasTemplatesAdded => SavedPages?.Count > 0 || SavedFeatures?.Count > 0;

        private void ValidateCurrentTemplateName(SavedTemplateViewModel item)
        {
            if (item.NewItemName != item.ItemName)
            {
                var validators = new List<Validator>()
                {
                    new ExistingNamesValidator(Names),
                    new ReservedNamesValidator()
                };
                if (item.CanChooseItemName)
                {
                    validators.Add(new DefaultNamesValidator());
                }
                var validationResult = Naming.Validate(item.NewItemName, validators);

                item.IsValidName = validationResult.IsValid;
                item.ErrorMessage = string.Empty;

                if (!item.IsValidName)
                {
                    item.ErrorMessage = StringRes.ResourceManager.GetString($"ValidationError_{validationResult.ErrorType}");

                    if (string.IsNullOrWhiteSpace(item.ErrorMessage))
                    {
                        item.ErrorMessage = "UndefinedError";
                    }
                    MainViewModel.Current.SetValidationErrors(item.ErrorMessage);
                    throw new Exception(item.ErrorMessage);
                }
            }
            MainViewModel.Current.CleanStatus(true);
        }

        private void ValidateNewTemplateName(TemplateInfoViewModel template)
        {
            var validators = new List<Validator>()
            {
                new ExistingNamesValidator(Names),
                new ReservedNamesValidator()
            };
            if (template.CanChooseItemName)
            {
                validators.Add(new DefaultNamesValidator());
            }
            var validationResult = Naming.Validate(template.NewTemplateName, validators);

            template.IsValidName = validationResult.IsValid;
            template.ErrorMessage = string.Empty;

            if (!template.IsValidName)
            {
                template.ErrorMessage = StringRes.ResourceManager.GetString($"ValidationError_{validationResult.ErrorType}");

                if (string.IsNullOrWhiteSpace(template.ErrorMessage))
                {
                    template.ErrorMessage = "UndefinedError";
                }
                MainViewModel.Current.SetValidationErrors(template.ErrorMessage);
                throw new Exception(template.ErrorMessage);
            }
            MainViewModel.Current.CleanStatus(true);
        }

        public async Task InitializeAsync()
        {
            MainViewModel.Current.Title = StringRes.ProjectTemplatesTitle;
            ContextProjectType = MainViewModel.Current.ProjectSetup.SelectedProjectType;
            ContextFramework = MainViewModel.Current.ProjectSetup.SelectedFramework;

            if (PagesGroups.Count == 0)
            {
                var pages = GenContext.ToolBox.Repo.Get(t => t.GetTemplateType() == TemplateType.Page && t.GetFrameworkList().Contains(ContextFramework.Name))
                                                   .Select(t => new TemplateInfoViewModel(t, GenComposer.GetAllDependencies(t, ContextFramework.Name), AddTemplateCommand, SaveTemplateCommand, ValidateNewTemplateName));

                var groups = pages.GroupBy(t => t.Group).Select(gr => new GroupTemplateInfoViewModel(gr.Key as string, gr.ToList())).OrderBy(gr => gr.Title);

                PagesGroups.AddRange(groups);
                PagesHeader = string.Format(StringRes.GroupPagesHeader_SF, pages.Count());
            }

            if (FeatureGroups.Count == 0)
            {
                var features = GenContext.ToolBox.Repo.Get(t => t.GetTemplateType() == TemplateType.Feature && t.GetFrameworkList().Contains(ContextFramework.Name) && !t.GetIsHidden())
                                                      .Select(t => new TemplateInfoViewModel(t, GenComposer.GetAllDependencies(t, ContextFramework.Name), AddTemplateCommand, SaveTemplateCommand, ValidateNewTemplateName));

                var groups = features.GroupBy(t => t.Group).Select(gr => new GroupTemplateInfoViewModel(gr.Key as string, gr.ToList())).OrderBy(gr => gr.Title);

                FeatureGroups.AddRange(groups);
                FeaturesHeader = string.Format(StringRes.GroupFeaturesHeader_SF, features.Count());
            }

            if (SavedPages.Count == 0 && SavedFeatures.Count == 0)
            {
                SetupTemplatesFromLayout(ContextProjectType.Name, ContextFramework.Name);
                MainViewModel.Current.RebuildLicenses();
            }
            MainViewModel.Current.SetTemplatesReadyForProjectCreation();
            CloseTemplatesEdition();
            await Task.CompletedTask;
        }

        public void ResetSelection()
        {
            SavedPages.Clear();
            SavedFeatures.Clear();
            PagesGroups.Clear();
            FeatureGroups.Clear();
        }

        private void OnAddTemplateItem(TemplateInfoViewModel template)
        {
            if (template.CanChooseItemName)
            {
                var validators = new List<Validator>()
                {
                    new ReservedNamesValidator(),
                    new ExistingNamesValidator(Names),
                    new DefaultNamesValidator()
                };
                template.NewTemplateName = Naming.Infer(template.Template.GetDefaultName(), validators);
                CloseTemplatesEdition();
                template.IsEditionEnabled = true;
            }
            else
            {
                var validators = new List<Validator>()
                {
                    new ReservedNamesValidator(),
                    new ExistingNamesValidator(Names)
                };
                template.NewTemplateName = Naming.Infer(template.Template.GetDefaultName(), validators);
                SetupTemplateAndDependencies((template.NewTemplateName, template.Template));
                var isAlreadyDefined = IsTemplateAlreadyDefined(template.Template.Identity);
                template.UpdateTemplateAvailability(isAlreadyDefined);
            }
        }

        private void OnSaveTemplateItem(TemplateInfoViewModel template)
        {
            if (template.IsValidName)
            {
                SetupTemplateAndDependencies((template.NewTemplateName, template.Template));
                template.CloseEdition();

                var isAlreadyDefined = IsTemplateAlreadyDefined(template.Template.Identity);
                template.UpdateTemplateAvailability(isAlreadyDefined);
            }
        }

        private void OnRenameSummaryItem(SavedTemplateViewModel item)
        {
            CloseSummaryItemsEdition();
            item.IsEditionEnabled = true;
            item.TryClose();
        }

        private void OnConfirmRenameSummaryItem(SavedTemplateViewModel item)
        {
            if (item.NewItemName == item.ItemName)
            {
                item.IsEditionEnabled = false;
                return;
            }
            var validators = new List<Validator>()
            {
                new ExistingNamesValidator(Names),
                new ReservedNamesValidator()
            };
            if (item.CanChooseItemName)
            {
                validators.Add(new DefaultNamesValidator());
            }
            var validationResult = Naming.Validate(item.NewItemName, validators);

            if (validationResult.IsValid)
            {
                item.ItemName = item.NewItemName;
                item.IsEditionEnabled = false;

                if (item.IsHome)
                {
                    HomeName = item.ItemName;
                }

                AppHealth.Current.Telemetry.TrackEditSummaryItem(EditItemActionEnum.Rename).FireAndForget();
            }
        }

        private void OnOpenSummaryItem(SavedTemplateViewModel item)
        {
            if (!item.IsOpen)
            {
                SavedPages.ToList().ForEach(pg => pg.ToList().ForEach(p => TryClose(p, item)));
                SavedFeatures.ToList().ForEach(f => TryClose(f, item));
                item.IsOpen = true;
            }
            else
            {
                item.TryClose();
            }
        }

        private void TryClose(SavedTemplateViewModel target, SavedTemplateViewModel origin)
        {
            if (target.IsOpen && target.ItemName != origin.ItemName)
            {
                target.TryClose();
            }
        }

        public void SetHomePage(SavedTemplateViewModel item)
        {
            if (!item.IsHome)
            {
                foreach (var spg in SavedPages)
                {
                    spg.ToList().ForEach(sp => sp.TryReleaseHome());
                }

                item.IsHome = true;
                HomeName = item.ItemName;
                AppHealth.Current.Telemetry.TrackEditSummaryItem(EditItemActionEnum.SetHome).FireAndForget();
            }
        }

        private void OnRemoveTemplate(SavedTemplateViewModel item)
        {
            var dependencyItem = SavedPages[item.GenGroup].FirstOrDefault(st => st.DependencyList.Any(d => d == item.Identity));
            if (dependencyItem == null)
            {
                dependencyItem = SavedFeatures.FirstOrDefault(st => st.DependencyList.Any(d => d == item.Identity));
            }
            if (dependencyItem != null)
            {
                string message = string.Format(StringRes.ValidationError_CanNotRemoveTemplate_SF, item.TemplateName, dependencyItem.TemplateName, dependencyItem.TemplateType);
                MainViewModel.Current.Status = new StatusViewModel(Controls.StatusType.Warning, message, true);
                return;
            }
            if (SavedPages[item.GenGroup].Contains(item))
            {
                SavedPages[item.GenGroup].Remove(item);
            }
            else if (SavedFeatures.Contains(item))
            {
                SavedFeatures.Remove(item);
            }
            MainViewModel.Current.CreateCommand.OnCanExecuteChanged();
            UpdateTemplatesAvailability();
            MainViewModel.Current.RebuildLicenses();

            AppHealth.Current.Telemetry.TrackEditSummaryItem(EditItemActionEnum.Remove).FireAndForget();
        }

        private bool IsTemplateAlreadyDefined(string identity) => Identities.Any(i => i == identity);

        public void CloseTemplatesEdition()
        {
            PagesGroups.ToList().ForEach(g => g.Templates.ToList().ForEach(t => t.CloseEdition()));
            FeatureGroups.ToList().ForEach(g => g.Templates.ToList().ForEach(t => t.CloseEdition()));
        }

        public void CloseSummaryItemsEdition()
        {
            SavedPages.ToList().ForEach(spg => spg.ToList().ForEach(p => p.OnCancelRename()));
            SavedFeatures.ToList().ForEach(f => f.OnCancelRename());
        }

        public void DropTemplate(object sender, DragAndDropEventArgs<SavedTemplateViewModel> e)
        {
            if (SavedPages.Count > 0 && SavedPages.Count >= e.ItemData.GenGroup + 1)
            {
                var items = SavedPages[e.ItemData.GenGroup];
                if (items.Count > 1)
                {
                    if (e.NewIndex == 0)
                    {
                        SetHomePage(e.ItemData);
                    }
                    if (e.OldIndex > -1)
                    {
                        SavedPages[e.ItemData.GenGroup].Move(e.OldIndex, e.NewIndex);
                    }
                    SetHomePage(items.First());
                }
            }
        }

        private void UpdateTemplatesAvailability()
        {
            PagesGroups.ToList().ForEach(g => g.Templates.ToList().ForEach(t =>
            {
                var isAlreadyDefined = IsTemplateAlreadyDefined(t.Template.Identity);
                t.UpdateTemplateAvailability(isAlreadyDefined);
            }));

            FeatureGroups.ToList().ForEach(g => g.Templates.ToList().ForEach(t =>
            {
                var isAlreadyDefined = IsTemplateAlreadyDefined(t.Template.Identity);
                t.UpdateTemplateAvailability(isAlreadyDefined);
            }));
        }

        private void UpdateSummaryTemplates()
        {
            foreach (var spg in SavedPages)
            {
                spg.ToList().ForEach(sp => sp.UpdateAllowDragAndDrop(SavedPages[0].Count));
            }
        }

        private void SetupTemplatesFromLayout(string projectTypeName, string frameworkName)
        {
            var layout = GenComposer.GetLayoutTemplates(projectTypeName, frameworkName);

            foreach (var item in layout)
            {
                if (item.Template != null)
                {
                    SetupTemplateAndDependencies((item.Layout.name, item.Template), !item.Layout.@readonly);
                }
            }
        }

        private void SetupTemplateAndDependencies((string name, ITemplateInfo template) item, bool isRemoveEnabled = true)
        {
            SaveNewTemplate(item, isRemoveEnabled);
            var dependencies = GenComposer.GetAllDependencies(item.template, ContextFramework.Name);

            foreach (var dependencyTemplate in dependencies)
            {
                if (!Identities.Any(i => i == dependencyTemplate.Identity))
                {
                    SaveNewTemplate((dependencyTemplate.GetDefaultName(), dependencyTemplate), isRemoveEnabled);
                }
            }

            MainViewModel.Current.RebuildLicenses();
        }

        private void SaveNewTemplate((string name, ITemplateInfo template) item, bool isRemoveEnabled = true)
        {
            var newItem = new SavedTemplateViewModel(item, isRemoveEnabled, OpenSummaryItemCommand, RemoveTemplateCommand, RenameSummaryItemCommand, ConfirmRenameSummaryItemCommand, ValidateCurrentTemplateName);

            if (item.template.GetTemplateType() == TemplateType.Page)
            {
                if (SavedPages.Count == 0)
                {
                    HomeName = item.name;
                    newItem.IsHome = true;
                }
                while (SavedPages.Count < newItem.GenGroup + 1)
                {
                    var items = new ObservableCollection<SavedTemplateViewModel>();
                    SavedPages.Add(items);
                    MainViewModel.Current.DefineDragAndDrop(items, SavedPages.Count == 1);
                }
                SavedPages[newItem.GenGroup].Add(newItem);
            }
            else if (item.template.GetTemplateType() == TemplateType.Feature)
            {
                SavedFeatures.Add(newItem);
            }
            UpdateTemplatesAvailability();
            UpdateSummaryTemplates();
        }
    }
}
