using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.Wizard.Host;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Microsoft.Templates.Wizard.ViewModels
{
    public class TemplateViewModel : ObservableBase
    {
        public TemplateViewModel()
        {

        }

        public TemplateViewModel(ITemplateInfo ti, IEnumerable<ITemplateInfo> dependencies)
        {
            Info = ti;

            var icon = ti.GetIcon();
            Icon = Extensions.CreateIcon(icon);
            Name = ti.Name;
            Description = ti.Description;
            Author = ti.Author;
            Version = ti.GetVersion();
            Order = ti.GetOrder();
            MultipleInstances = ti.GetMultipleInstance();
            //LicenceTerms = ti.GetLicenceTerms();
            Dependencies = string.Join(",", dependencies.Select(d => d.Name));
            
        }


        public ITemplateInfo Info { get; }

        private BitmapImage _icon;
        public BitmapImage Icon
        {
            get { return _icon; }
            set { SetProperty(ref _icon, value); }
        }

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
            set { SetProperty( ref _dependencies, value); }
        }

    }
}
