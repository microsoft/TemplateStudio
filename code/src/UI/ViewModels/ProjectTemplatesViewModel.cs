using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Templates.UI.ViewModels
{
    public class ProjectTemplatesViewModel : Observable
    {
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

        public ObservableCollection<TemplateInfoViewModel> Pages { get; } = new ObservableCollection<TemplateInfoViewModel>();
        public ObservableCollection<TemplateInfoViewModel> Features { get; } = new ObservableCollection<TemplateInfoViewModel>();

        public ObservableCollection<SummaryItemViewModel> SummaryPages { get; } = new ObservableCollection<SummaryItemViewModel>();
        public ObservableCollection<SummaryItemViewModel> SummaryFeatures { get; } = new ObservableCollection<SummaryItemViewModel>();

        public List<(string Name, ITemplateInfo Template)> SavedTemplates { get; } = new List<(string Name, ITemplateInfo Template)>();

        public IEnumerable<(string Name, ITemplateInfo Template)> SavedFeatures { get => SavedTemplates.Where(st => st.Template.GetTemplateType() == TemplateType.Feature); }
        public IEnumerable<(string Name, ITemplateInfo Template)> SavedPages { get => SavedTemplates.Where(st => st.Template.GetTemplateType() == TemplateType.Page); }

        private RelayCommand<SummaryItemViewModel> _removeItemCommand;
        public RelayCommand<SummaryItemViewModel> RemoveItemCommand => _removeItemCommand ?? (_removeItemCommand = new RelayCommand<SummaryItemViewModel>(RemoveItem));

        private RelayCommand<(string Name, ITemplateInfo Template)> _addCommand;
        public RelayCommand<(string Name, ITemplateInfo Template)> AddCommand => _addCommand ?? (_addCommand = new RelayCommand<(string Name, ITemplateInfo Template)>(OnAddItem));

        public Func<IEnumerable<string>> GetUsedNamesFunc => () => SavedTemplates.Select(t => t.Name);
        public Func<IEnumerable<string>> GetUsedTemplatesIdentitiesFunc => () => SavedTemplates.Select(t => t.Template.Identity);


        public async Task IniatializeAsync(string projectTypeName, string frameworkName)
        {
            if (Pages.Count == 0)
            {
                var pageTemplates = GenContext.ToolBox.Repo.Get(t => t.GetTemplateType() == TemplateType.Page && t.GetFrameworkList().Contains(frameworkName))
                                                            .Select(t => new TemplateInfoViewModel(t, GenContext.ToolBox.Repo.GetDependencies(t)))
                                                            .OrderBy(t => t.Order)
                                                            .ToList();

                foreach (var pageTemplate in pageTemplates)
                {                    
                    Pages.Add(pageTemplate);
                }
                PagesHeader = String.Format(StringRes.GroupPagesHeader_SF, Pages.Count);
            }

            if (Features.Count == 0)
            {
                var featureTemplates = GenContext.ToolBox.Repo.Get(t => t.GetTemplateType() == TemplateType.Feature && t.GetFrameworkList().Contains(frameworkName))
                                                            .Select(t => new TemplateInfoViewModel(t, GenContext.ToolBox.Repo.GetDependencies(t)))
                                                            .OrderBy(t => t.Order)
                                                            .ToList();
                foreach (var featureTemplate in featureTemplates)
                {
                    Features.Add(featureTemplate);
                }
                FeaturesHeader = String.Format(StringRes.GroupFeaturesHeader_SF, Pages.Count);
            }                       

            if (SavedTemplates == null || SavedTemplates.Count == 0)
            {
                AddFromLayout(projectTypeName, frameworkName);
            }

            await Task.CompletedTask;
        }

        private void AddFromLayout(string projectTypeName, string frameworkName)
        {
            var projectTemplate = GenContext.ToolBox.Repo.Find(t => t.GetProjectType() == projectTypeName && t.GetFrameworkList().Any(f => f == frameworkName));
            var layout = projectTemplate.GetLayout();

            foreach (var item in layout)
            {
                var template = GenContext.ToolBox.Repo.GetLayoutTemplate(item, frameworkName);
                if (template != null && template.GetTemplateType() == TemplateType.Page)
                {
                    SaveNewTemplate((item.name, template), !item.@readonly);
                }
            }
        }

        private void OnAddItem((string Name, ITemplateInfo Template) item) => SaveNewTemplate(item);


        private void SaveNewTemplate((string Name, ITemplateInfo Template) item, bool isRemoveEnabled = true)
        {
            SavedTemplates.Add(item);
            if (item.Template.GetTemplateType() == TemplateType.Page)
            {
                SummaryPages.Add(new SummaryItemViewModel()
                {
                    ItemName = item.Name,
                    TemplateName = item.Template.Name,
                    Author = item.Template.Author,
                    IsRemoveEnabled = isRemoveEnabled
                });
            }
            else if (item.Template.GetTemplateType() == TemplateType.Feature)
            {
                SummaryFeatures.Add(new SummaryItemViewModel()
                {
                    ItemName = item.Name,
                    TemplateName = item.Template.Name,
                    Author = item.Template.Author,
                    IsRemoveEnabled = isRemoveEnabled
                });
            }
        }

        private void RemoveItem(SummaryItemViewModel item)
        {
            if (SummaryPages.Contains(item))
            {
                SummaryPages.Remove(item);
            }
            else if (SummaryFeatures.Contains(item))
            {
                SummaryFeatures.Remove(item);
            }            
            SavedTemplates.Remove(SavedTemplates.First(st => st.Name == item.ItemName));
            OnPropertyChanged("GetUsedTemplatesIdentitiesFunc");
        }
    }
}