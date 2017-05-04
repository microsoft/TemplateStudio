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
using System.Windows;

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.Resources;
using Microsoft.Templates.UI.Views;

namespace Microsoft.Templates.UI.ViewModels
{
    public class ProjectTemplatesViewModel : Observable
    {
        public event EventHandler UpdateTemplateAvailable;
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

        public ObservableCollection<GroupTemplateInfoViewModel> PagesGroups { get; } = new ObservableCollection<GroupTemplateInfoViewModel>();
        public ObservableCollection<GroupTemplateInfoViewModel> FeatureGroups { get; } = new ObservableCollection<GroupTemplateInfoViewModel>();

        public ObservableCollection<SummaryItemViewModel> SummaryPages { get; } = new ObservableCollection<SummaryItemViewModel>();
        public ObservableCollection<SummaryItemViewModel> SummaryFeatures { get; } = new ObservableCollection<SummaryItemViewModel>();

        public List<(string Name, ITemplateInfo Template)> SavedTemplates { get; } = new List<(string Name, ITemplateInfo Template)>();

        public IEnumerable<(string Name, ITemplateInfo Template)> SavedFeatures { get => SavedTemplates.Where(st => st.Template.GetTemplateType() == TemplateType.Feature); }
        public IEnumerable<(string Name, ITemplateInfo Template)> SavedPages { get => SavedTemplates.Where(st => st.Template.GetTemplateType() == TemplateType.Page); }

        private RelayCommand<SummaryItemViewModel> _removeItemCommand;
        public RelayCommand<SummaryItemViewModel> RemoveItemCommand => _removeItemCommand ?? (_removeItemCommand = new RelayCommand<SummaryItemViewModel>(RemoveItem));

        private RelayCommand<(string Name, ITemplateInfo Template)> _addCommand;
        public RelayCommand<(string Name, ITemplateInfo Template)> AddCommand => _addCommand ?? (_addCommand = new RelayCommand<(string Name, ITemplateInfo Template)>((item)=> { OnAddItem(item); }));

        private RelayCommand<TemplateInfoViewModel> _showInfoCommand;
        public RelayCommand<TemplateInfoViewModel> ShowInfoCommand => _showInfoCommand ?? (_showInfoCommand = new RelayCommand<TemplateInfoViewModel>((template) => { OnShowInfo(template); }));

        public Func<IEnumerable<string>> GetUsedNamesFunc => () => SavedTemplates.Select(t => t.Name);
        public Func<IEnumerable<string>> GetUsedTemplatesIdentitiesFunc => () => SavedTemplates.Select(t => t.Template.Identity);

        public ProjectTemplatesViewModel()
        {
            SummaryFeatures.CollectionChanged += (s, o) => { OnPropertyChanged(nameof(SummaryFeatures)); };
            SummaryPages.CollectionChanged += (s, o) => { OnPropertyChanged(nameof(SummaryPages)); };
        }

        public async Task InitializeAsync()
        {
            MainViewModel.Current.Title = StringRes.ProjectTemplatesTitle;
            ContextProjectType = MainViewModel.Current.ProjectSetup.SelectedProjectType;
            ContextFramework = MainViewModel.Current.ProjectSetup.SelectedFramework;
            
            if (PagesGroups.Count == 0)
            {
                var pages = GenContext.ToolBox.Repo.Get(t => t.GetTemplateType() == TemplateType.Page && t.GetFrameworkList().Contains(ContextFramework.Name))
                                                   .Select(t => new TemplateInfoViewModel(t, GenComposer.GetAllDependencies(t, ContextFramework.Name)));

                var groups = pages.GroupBy(t => t.Group).Select(gr => new GroupTemplateInfoViewModel(gr.Key as string, gr.ToList())).OrderBy(gr => gr.Title);

                PagesGroups.AddRange(groups);
                PagesHeader = String.Format(StringRes.GroupPagesHeader_SF, pages.Count());
            }

            if (FeatureGroups.Count == 0)
            {
                var features = GenContext.ToolBox.Repo.Get(t => t.GetTemplateType() == TemplateType.Feature && t.GetFrameworkList().Contains(ContextFramework.Name))
                                                      .Select(t => new TemplateInfoViewModel(t, GenComposer.GetAllDependencies(t, ContextFramework.Name)));

                var groups = features.GroupBy(t => t.Group).Select(gr => new GroupTemplateInfoViewModel(gr.Key as string, gr.ToList())).OrderBy(gr => gr.Title);

                FeatureGroups.AddRange(groups);
                FeaturesHeader = String.Format(StringRes.GroupFeaturesHeader_SF, features.Count());
            }

            if (SavedTemplates == null || SavedTemplates.Count == 0)
            {
                AddFromLayout(ContextProjectType.Name, ContextFramework.Name);
                MainViewModel.Current.RebuildLicenses();
            }
            MainViewModel.Current.EnableProjectCreation();
            await Task.CompletedTask;
        }

        internal void ResetSelection()
        {
            SummaryPages.Clear();
            SummaryFeatures.Clear();
            SavedTemplates.Clear();
            PagesGroups.Clear();
            FeatureGroups.Clear();
        }

        private void AddFromLayout(string projectTypeName, string frameworkName)
        {
            var layout = GenComposer.GetLayoutTemplates(projectTypeName, frameworkName);

            foreach (var item in layout)
            {
                if (item.Template != null)
                {
                    OnAddItem((item.Layout.name, item.Template), !item.Layout.@readonly);
                }
            }
        }

        private void OnAddItem((string Name, ITemplateInfo Template) item, bool isRemoveEnabled = true)
        {
            SaveNewTemplate(item, isRemoveEnabled);
            var dependencies = GenComposer.GetAllDependencies(item.Template, ContextFramework.Name);

            foreach (var dependencyTemplate in dependencies)
            {
                if (!SavedTemplates.Any(s => s.Template.Identity == dependencyTemplate.Identity))
                {
                    SaveNewTemplate((dependencyTemplate.GetDefaultName(), dependencyTemplate), isRemoveEnabled);
                }
            }

            MainViewModel.Current.RebuildLicenses();
        }

        private void OnShowInfo(TemplateInfoViewModel template)
        {
            MainViewModel.Current.InfoShapeVisibility = Visibility.Visible;
            var infoView = new InformationWindow(template, MainViewModel.Current.MainView);

            infoView.ShowDialog();
            MainViewModel.Current.InfoShapeVisibility = Visibility.Collapsed;

        }

        private void SaveNewTemplate((string Name, ITemplateInfo Template) item, bool isRemoveEnabled = true)
        {
            SavedTemplates.Add(item);

            var newItem = new SummaryItemViewModel
            {
                Author = item.Template.Author,
                HasDefaultName = !item.Template.GetItemNameEditable(),
                Identity = item.Template.Identity,
                IsRemoveEnabled = isRemoveEnabled,
                ItemName = item.Name,
                TemplateName = item.Template.Name,
            };

            if (item.Template.GetTemplateType() == TemplateType.Page)
            {
                SummaryPages.Add(newItem);
            }
            else if (item.Template.GetTemplateType() == TemplateType.Feature)
            {
                SummaryFeatures.Add(newItem);
            }

            UpdateTemplateAvailable?.Invoke(this, EventArgs.Empty);
        }

        private void RemoveItem(SummaryItemViewModel item)
        {
            if (SavedTemplates.Any(st => st.Template.GetDependencyList().Any(d => d == item.Identity)))
            {
                var dependencyName = SavedTemplates.First(st => st.Template.GetDependencyList().Any(d => d == item.Identity));
                string message = String.Format(StringRes.ValidationError_CanNotRemoveTemplate_SF, item.TemplateName, dependencyName.Template.Name, dependencyName.Template.GetTemplateType());

                MainViewModel.Current.Status = new StatusViewModel(Controls.StatusType.Warning, message, true);

                return;
            }
            if (SummaryPages.Contains(item))
            {
                SummaryPages.Remove(item);
            }
            else if (SummaryFeatures.Contains(item))
            {
                SummaryFeatures.Remove(item);
            }

            SavedTemplates.Remove(SavedTemplates.First(st => st.Name == item.ItemName));
            UpdateTemplateAvailable?.Invoke(this, EventArgs.Empty);

            MainViewModel.Current.RebuildLicenses();
        }
    }
}
