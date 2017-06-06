using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.UI.ViewModels.NewItem
{
    public class TemplateInfoViewModel : Observable
    {
        public bool IsItemNameEditable { get; set; }
        public string DefaultName { get; set; }
        public string Group { get; set; }

        private string _icon;
        public string Icon
        {
            get => _icon;
            set => SetProperty(ref _icon, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _author;
        public string Author
        {
            get => _author;
            set => SetProperty(ref _author, value);
        }

        private string _summary;
        public string Summary
        {
            get => _summary;
            set => SetProperty(ref _summary, value);
        }


        public TemplateInfoViewModel(ITemplateInfo template)
        {
            IsItemNameEditable = template.GetItemNameEditable();
            DefaultName = template.GetDefaultName();
            Group = template.GetGroup();
            Icon = template.GetIcon();
            Name = template.Name;
            Author = template.Author;
            Summary = template.Description;
        }
    }
}
