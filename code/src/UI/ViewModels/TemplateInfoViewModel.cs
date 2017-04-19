using Microsoft.Templates.Core.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;

namespace Microsoft.Templates.UI.ViewModels
{
    public class TemplateInfoViewModel : Observable
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }        

        private string _description;
        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        private string _author;
        public string Author
        {
            get { return _author; }
            set { SetProperty(ref _author, value); }
        }

        private string _icon;
        public string Icon
        {
            get { return _icon; }
            set { SetProperty(ref _icon, value); }
        }

        private string _version;
        public string Version
        {
            get { return _version; }
            set { SetProperty(ref _version, value); }
        }

        private int _order;
        public int Order
        {
            get { return _order; }
            set { SetProperty(ref _order, value); }
        }

        private bool _multipleInstances;
        public bool MultipleInstances
        {
            get { return _multipleInstances; }
            set { SetProperty(ref _multipleInstances, value); }
        }

        private string _licenceTerms;
        public string LicenceTerms
        {
            get { return _licenceTerms; }
            set { SetProperty(ref _licenceTerms, value); }
        }

        private string _dependencies;
        public string Dependencies
        {
            get { return _dependencies; }
            set { SetProperty(ref _dependencies, value); }
        }

        public TemplateInfoViewModel(ITemplateInfo template, IEnumerable<ITemplateInfo> dependencies)
        {
            Name = template.Name;
            Description = template.Description;
            Author = template.Author;
            Icon = template.GetIcon();
            Version = template.GetVersion();
            Order = template.GetOrder();
            MultipleInstances = template.GetMultipleInstance();
            Dependencies = string.Join(",", dependencies.Select(d => d.Name));
        }
    }
}
