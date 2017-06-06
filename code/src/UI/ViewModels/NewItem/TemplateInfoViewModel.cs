using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.UI.ViewModels.Common;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Microsoft.Templates.UI.ViewModels.NewItem
{
    public class TemplateInfoViewModel : CommonInfoViewModel
    {
        public ITemplateInfo Template { get; set; }
        public bool IsItemNameEditable { get; set; }
        public string DefaultName { get; set; }
        public string Group { get; set; }
        public string Identity { get; set; }
        public TemplateType TemplateType { get; set; }


        public ObservableCollection<DependencyInfoViewModel> DependencyItems { get; } = new ObservableCollection<DependencyInfoViewModel>();


        public TemplateInfoViewModel(ITemplateInfo template, IEnumerable<ITemplateInfo> dependencies)
        {
            Template = template;
            IsItemNameEditable = template.GetItemNameEditable();
            DefaultName = template.GetDefaultName();
            Group = template.GetGroup();
            Icon = template.GetIcon();
            Name = template.Name;
            Author = template.Author;
            Summary = template.Description;
            Identity = template.Identity;
            Version = template.GetVersion();
            TemplateType = template.GetTemplateType();
            Description = template.GetRichDescription();
            DependencyItems.AddRange(dependencies.Select(d => new DependencyInfoViewModel(new TemplateInfoViewModel(d, GenComposer.GetAllDependencies(d, MainViewModel.Current.ConfigFramework)))));
        }
    }
}
