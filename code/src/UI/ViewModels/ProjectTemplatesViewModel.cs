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

        public ObservableCollection<TemplateInfoViewModel> Pages { get; } = new ObservableCollection<TemplateInfoViewModel>();
        public ObservableCollection<TemplateInfoViewModel> Features { get; } = new ObservableCollection<TemplateInfoViewModel>();

        private RelayCommand<TemplateInfoViewModel> _addCommand;
        public RelayCommand<TemplateInfoViewModel> AddCommand => _addCommand ?? (_addCommand = new RelayCommand<TemplateInfoViewModel>(OnAddItem));        

        public async Task IniatializeAsync(string frameworkName)
        {
            var pageTemplates = GenContext.ToolBox.Repo.Get(t => t.GetTemplateType() == TemplateType.Page
                                                                && t.GetFrameworkList().Contains(frameworkName)
                                                                && (t.GetMultipleInstance() == true || !IsAlreadyDefined(t)))
                                                            .Select(t => new TemplateInfoViewModel(t, GenContext.ToolBox.Repo.GetDependencies(t)))
                                                            .OrderBy(t => t.Order)
                                                            .ToList();

            var featureTemplates = GenContext.ToolBox.Repo.Get(t => t.GetTemplateType() == TemplateType.Feature
                                                                && t.GetFrameworkList().Contains(frameworkName)
                                                                && (t.GetMultipleInstance() == true || !IsAlreadyDefined(t)))
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
            await Task.CompletedTask;
        }

        private bool IsAlreadyDefined(ITemplateInfo t)
        {
            return false;
        }

        private void OnAddItem(TemplateInfoViewModel item)
        {
        }
    }
}
