using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.Resources;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Microsoft.Templates.UI.ViewModels
{
    public class GroupTemplateInfoViewModel : Observable
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public ObservableCollection<TemplateInfoViewModel> Templates { get; } = new ObservableCollection<TemplateInfoViewModel>();

        public GroupTemplateInfoViewModel(string name, IEnumerable<TemplateInfoViewModel> templates)
        {
            Name = name;
            Title = GetTitle(name);
            Templates.AddRange(templates);            
        }

        private string GetTitle(string name) => StringRes.ResourceManager.GetString($"TemplateGroup_{name}");
    }
}
