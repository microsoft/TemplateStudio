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
        public MetadataInfoViewModel ContextFramework { get; private set; }
        public MetadataInfoViewModel ContextProjectType { get; private set; }

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

        // public string HomeName { get; set; }

        public ObservableCollection<ItemsGroupViewModel<TemplateInfoViewModel>> PagesGroups { get; } = new ObservableCollection<ItemsGroupViewModel<TemplateInfoViewModel>>();
        public ObservableCollection<ItemsGroupViewModel<TemplateInfoViewModel>> FeatureGroups { get; } = new ObservableCollection<ItemsGroupViewModel<TemplateInfoViewModel>>();

        public ObservableCollection<ObservableCollection<SavedTemplateViewModel>> SavedPages { get; } = new ObservableCollection<ObservableCollection<SavedTemplateViewModel>>();
        public ObservableCollection<SavedTemplateViewModel> SavedFeatures { get; } = new ObservableCollection<SavedTemplateViewModel>();

        private bool _hasSavedPages;
        public bool HasSavedPages
        {
            get => _hasSavedPages;
            set => SetProperty(ref _hasSavedPages, value);
        }

        private bool _hasSavedFeatures;
        public bool HasSavedFeatures
        {
            get => _hasSavedFeatures;
            set => SetProperty(ref _hasSavedFeatures, value);
        }

        public ProjectTemplatesViewModel()
        {
            SavedFeatures.CollectionChanged += (s, o) => { OnPropertyChanged(nameof(SavedFeatures)); };
            SavedPages.CollectionChanged += (s, o) => { OnPropertyChanged(nameof(SavedPages)); };
            ValidationService.Initialize(GetNames);
            UserSelectionService.Initialize(GetSavedPages, GetSavedFeatures);
            OrderingService.Initialize(GetSavedPages, SetHomePage);
        }

        private ObservableCollection<SavedTemplateViewModel> GetSavedFeatures() => SavedFeatures;
        public ObservableCollection<ObservableCollection<SavedTemplateViewModel>> GetSavedPages() => SavedPages;

        private IEnumerable<string> GetNames()
        {
            var names = new List<string>();
            SavedPages.ToList().ForEach(spg => names.AddRange(spg.Select(sp => sp.ItemName)));
            names.AddRange(SavedFeatures.Select(sf => sf.ItemName));
            return names;
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
                UserSelectionService.SetupTemplatesFromLayout(ContextProjectType.Name, ContextFramework.Name);
                UpdateTemplatesAvailability();
                UpdateSummaryTemplates();
                UpdateHasPagesAndHasFeatures();
                if (HasSavedPages)
                {
                    var firstPage = SavedPages.First().First();
                    UserSelectionService.HomeName = firstPage.ItemName;
                    firstPage.IsHome = true;
                }
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
                UserSelectionService.HomeName = item.ItemName;
                AppHealth.Current.Telemetry.TrackEditSummaryItemAsync(EditItemActionEnum.SetHome).FireAndForget();
            }
        }

        public bool IsTemplateAlreadyDefined(string identity)
        {
            var identities = new List<string>();
            SavedPages.ToList().ForEach(spg => identities.AddRange(spg.Select(sp => sp.Identity)));
            identities.AddRange(SavedFeatures.Select(sf => sf.Identity));
            return identities.Any(i => i == identity);
        }

        public void UpdateTemplatesAvailability()
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

        public void UpdateSummaryTemplates()
        {
            foreach (var spg in SavedPages)
            {
                spg.ToList().ForEach(sp => sp.UpdateAllowDragAndDrop(SavedPages[0].Count));
            }
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

        public void UpdateHasPagesAndHasFeatures()
        {
            HasSavedPages = SavedPages.Any(g => g.Any());
            HasSavedFeatures = SavedFeatures.Any();
        }
    }
}
