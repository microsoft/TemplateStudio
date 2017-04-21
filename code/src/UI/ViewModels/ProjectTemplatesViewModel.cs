using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.UI.Resources;

namespace Microsoft.Templates.UI.ViewModels
{
    public class ProjectTemplatesViewModel : Observable
    {
        private string _pagesHeader;
        public string PagesHeader
        {
            get { return _pagesHeader; }
            set { SetProperty(ref _pagesHeader, value); }
        }

        private string _featuresHeader;
        public string FeaturesHeader
        {
            get { return _featuresHeader; }
            set { SetProperty(ref _featuresHeader, value); }
        }
        public Func<IEnumerable<string>> GetUsedNamesFunc => () => SavedTemplates.Select(t => t.Name);
        public Func<IEnumerable<string>> GetUsedTemplatesIdentitiesFunc => () => SavedTemplates.Select(t => t.Template.Identity);

        public ObservableCollection<TemplateInfoViewModel> Pages { get; } = new ObservableCollection<TemplateInfoViewModel>();
        public ObservableCollection<TemplateInfoViewModel> Features { get; } = new ObservableCollection<TemplateInfoViewModel>();

        public List<(string Name, ITemplateInfo Template)> SavedTemplates { get; } = new List<(string Name, ITemplateInfo Template)>();

        private RelayCommand<(string Name, ITemplateInfo Template)> _addCommand;
        public RelayCommand<(string Name, ITemplateInfo Template)> AddCommand => _addCommand ?? (_addCommand = new RelayCommand<(string Name, ITemplateInfo Template)>(OnAddItem));

        public async Task IniatializeAsync(string projectTypeName, string frameworkName)
        {
            var pageTemplates = GenContext.ToolBox.Repo.Get(t => t.GetTemplateType() == TemplateType.Page && t.GetFrameworkList().Contains(frameworkName))
                                                            .Select(t => new TemplateInfoViewModel(t, GenContext.ToolBox.Repo.GetDependencies(t)))
                                                            .OrderBy(t => t.Order)
                                                            .ToList();

            var featureTemplates = GenContext.ToolBox.Repo.Get(t => t.GetTemplateType() == TemplateType.Feature && t.GetFrameworkList().Contains(frameworkName))
                                                            .Select(t => new TemplateInfoViewModel(t, GenContext.ToolBox.Repo.GetDependencies(t)))
                                                            .OrderBy(t => t.Order)
                                                            .ToList();
            

            Pages.Clear();
            Features.Clear();

            foreach (var pageTemplate in pageTemplates)
            {
                Pages.Add(pageTemplate);
            }

            foreach (var featureTemplate in featureTemplates)
            {
                Features.Add(featureTemplate);
            }

            PagesHeader = String.Format(StringRes.GroupPagesHeader_SF, Pages.Count);
            FeaturesHeader = String.Format(StringRes.GroupFeaturesHeader_SF, Pages.Count);

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
                    SavedTemplates.Add((item.name, template));
                }
            }
        }        

        private void OnAddItem((string Name, ITemplateInfo Template) item)
        {
            SavedTemplates.Add(item);
        }        
    }
}
