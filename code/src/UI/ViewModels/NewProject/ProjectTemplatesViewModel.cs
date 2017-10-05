// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
using Microsoft.Templates.UI.ViewModels.Common;

namespace Microsoft.Templates.UI.ViewModels.NewProject
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

        public ObservableCollection<ItemsGroupViewModel<TemplateInfoViewModel>> PagesGroups { get; } = new ObservableCollection<ItemsGroupViewModel<TemplateInfoViewModel>>();
        public ObservableCollection<ItemsGroupViewModel<TemplateInfoViewModel>> FeatureGroups { get; } = new ObservableCollection<ItemsGroupViewModel<TemplateInfoViewModel>>();

        public ObservableCollection<ObservableCollection<SavedTemplateViewModel>> SavedPages { get; } = new ObservableCollection<ObservableCollection<SavedTemplateViewModel>>();
        public ObservableCollection<SavedTemplateViewModel> SavedFeatures { get; } = new ObservableCollection<SavedTemplateViewModel>();

        private bool _hasSavedPages;
        public bool HasSavedPages
        {
            get => _hasSavedPages;
            private set => SetProperty(ref _hasSavedPages, value);
        }

        private bool _hasSavedFeatures;
        public bool HasSavedFeatures
        {
            get => _hasSavedFeatures;
            private set => SetProperty(ref _hasSavedFeatures, value);
        }

        public ProjectTemplatesViewModel()
        {
            SavedFeatures.CollectionChanged += (s, o) => { OnPropertyChanged(nameof(SavedFeatures)); };
            SavedPages.CollectionChanged += (s, o) => { OnPropertyChanged(nameof(SavedPages)); };
            ValidationService.Initialize(GetNames);
        }

        private IEnumerable<string> GetNames()
        {
            var names = new List<string>();
            SavedPages.ToList().ForEach(spg => names.AddRange(spg.Select(sp => sp.ItemName)));
            names.AddRange(SavedFeatures.Select(sf => sf.ItemName));
            return names;
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

        public async Task InitializeAsync()
        {
            ContextProjectType = MainViewModel.Current.ProjectSetup.SelectedProjectType;
            ContextFramework = MainViewModel.Current.ProjectSetup.SelectedFramework;

            var totalPages = DataService.LoadTemplatesGroups(PagesGroups, TemplateType.Page, ContextFramework.Name);
            if (totalPages > 0)
            {
                PagesHeader = string.Format(StringRes.GroupPagesHeader_SF, totalPages);
            }

            var totalFeatures = DataService.LoadTemplatesGroups(FeatureGroups, TemplateType.Feature, ContextFramework.Name);
            if (totalFeatures > 0)
            {
                FeaturesHeader = string.Format(StringRes.GroupFeaturesHeader_SF, totalFeatures);
            }

            if (SavedPages.Count == 0 && SavedFeatures.Count == 0)
            {
                SetupTemplatesFromLayout(ContextProjectType.Name, ContextFramework.Name);
                MainViewModel.Current.RebuildLicenses();
            }

            CloseAllEditions();
            await Task.CompletedTask;
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
                AppHealth.Current.Telemetry.TrackEditSummaryItemAsync(EditItemActionEnum.SetHome).FireAndForget();
            }
        }

        public void UpdateHomePageName(string name) => HomeName = name;

        public void RemoveTemplate(SavedTemplateViewModel item, bool showErrors)
        {
            // Look if is there any templates that depends on item
            if (AnySavedTemplateDependsOnItem(item, showErrors))
            {
                return;
            }

            // Remove template
            if (SavedPages[item.GenGroup].Contains(item))
            {
                SavedPages[item.GenGroup].Remove(item);
                HasSavedPages = SavedPages.Any(g => g.Any());
            }
            else if (SavedFeatures.Contains(item))
            {
                SavedFeatures.Remove(item);
                HasSavedFeatures = SavedFeatures.Any();
            }

            TryRemoveHiddenDependencies(item);

            MainViewModel.Current.FinishCommand.OnCanExecuteChanged();
            UpdateTemplatesAvailability();
            MainViewModel.Current.RebuildLicenses();

            AppHealth.Current.Telemetry.TrackEditSummaryItemAsync(EditItemActionEnum.Remove).FireAndForget();
        }

        private bool AnySavedTemplateDependsOnItem(SavedTemplateViewModel item, bool showErrors)
        {
            SavedTemplateViewModel dependencyItem = null;
            foreach (var group in SavedPages)
            {
                dependencyItem = group.FirstOrDefault(st => st.DependencyList.Any(d => d == item.Identity));
                if (dependencyItem != null)
                {
                    break;
                }
            }

            if (dependencyItem == null)
            {
                dependencyItem = SavedFeatures.FirstOrDefault(st => st.DependencyList.Any(d => d == item.Identity));
            }

            if (dependencyItem != null)
            {
                if (showErrors)
                {
                    string message = string.Format(StringRes.ValidationError_CanNotRemoveTemplate_SF, item.TemplateName, dependencyItem.TemplateName, dependencyItem.TemplateType);
                    MainViewModel.Current.WizardStatus.SetStatus(StatusViewModel.Warning(message, false, 5));
                }

                return true;
            }

            return false;
        }

        private void TryRemoveHiddenDependencies(SavedTemplateViewModel item)
        {
            foreach (var identity in item.DependencyList)
            {
                var dependency = SavedFeatures.FirstOrDefault(sf => sf.Identity == identity);
                if (dependency == null)
                {
                    foreach (var pageGroup in SavedPages)
                    {
                        dependency = pageGroup.FirstOrDefault(sf => sf.Identity == identity);
                        if (dependency != null)
                        {
                            break;
                        }
                    }
                }

                if (dependency != null)
                {
                    // If the template is not hidden we can not remove it because it could be added in wizard
                    if (dependency.IsHidden)
                    {
                        // Look if there are another saved template that depends on it.
                        // For example, if it's added two different chart pages, when remove the first one SampleDataService can not be removed, but if no saved templates use SampleDataService, it can be removed.
                        if (!SavedFeatures.Any(sf => sf.DependencyList.Any(d => d == dependency.Identity)) || SavedPages.Any(spg => spg.Any(sp => sp.DependencyList.Any(d => d == dependency.Identity))))
                        {
                            RemoveTemplate(dependency, false);
                        }
                    }
                }
            }
        }

        public bool IsTemplateAlreadyDefined(string identity) => Identities.Any(i => i == identity);

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
                    AddTemplateAndDependencies((item.Layout.name, item.Template), !item.Layout.@readonly);
                }
            }
        }

        public void AddTemplateAndDependencies((string name, ITemplateInfo template) item, bool isRemoveEnabled = true)
        {
            SaveNewTemplate(item, isRemoveEnabled);

            foreach (var dependencyTemplate in GenComposer.GetAllDependencies(item.template, ContextFramework.Name))
            {
                SaveNewTemplate((dependencyTemplate.GetDefaultName(), dependencyTemplate), isRemoveEnabled);
            }

            MainViewModel.Current.RebuildLicenses();
        }

        private void SaveNewTemplate((string name, ITemplateInfo template) item, bool isRemoveEnabled = true)
        {
            if (item.template.GetMultipleInstance() == false && IsTemplateAlreadyDefined(item.template.Identity))
            {
                return;
            }

            var newItem = new SavedTemplateViewModel(item, isRemoveEnabled);

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
                    MainViewModel.Current.Ordering.AddList(items, SavedPages.Count == 1);
                }

                SavedPages[newItem.GenGroup].Add(newItem);
                HasSavedPages = true;
            }
            else if (item.template.GetTemplateType() == TemplateType.Feature)
            {
                SavedFeatures.Add(newItem);
                HasSavedFeatures = true;
            }

            UpdateTemplatesAvailability();
            UpdateSummaryTemplates();
        }

        // UI Changes
        public bool CloseAllEditions()
        {
            // Try to cancel edition on all templates and return true if any template was editing
            bool isEditingTemplate = false;
            PagesGroups.ToList().ForEach(g => g.Templates.ToList().ForEach(t =>
            {
                if (t.CloseEdition())
                {
                    isEditingTemplate = true;
                }
            }));
            FeatureGroups.ToList().ForEach(g => g.Templates.ToList().ForEach(t =>
            {
                if (t.CloseEdition())
                {
                    isEditingTemplate = true;
                }
            }));
            return isEditingTemplate;
        }

        public void CancelAllRenaming()
        {
            SavedPages.ToList().ForEach(spg => spg.ToList().ForEach(p => p.CancelRename()));
            SavedFeatures.ToList().ForEach(f => f.CancelRename());
        }

        public void CloseAllButThis(SavedTemplateViewModel item)
        {
            SavedPages.ToList().ForEach(pg => pg.ToList().ForEach(p => TryClose(p, item)));
            SavedFeatures.ToList().ForEach(f => TryClose(f, item));
        }

        private void TryClose(SavedTemplateViewModel target, SavedTemplateViewModel origin)
        {
            if (target.IsOpen && target.ItemName != origin.ItemName)
            {
                target.Close();
            }
        }
    }
}
