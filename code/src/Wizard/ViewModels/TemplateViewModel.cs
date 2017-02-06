using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using System;
using System.Collections.Generic;
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

        public TemplateViewModel(ITemplateInfo ti)
        {
            Info = ti;

            var icon = ti.GetIcon();
            Icon = Extensions.CreateIcon(icon);
            Name = ti.Name;
            Description = ti.Description;
            Author = ti.Author;
            Version = ti.GetVersion();
            Order = ti.GetOrder();
            //LicenceTerms = ti.GetLicenceTerms();
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

        private string _licenceTerms;
        public string LicenceTerms
        {
            get { return _licenceTerms; }
            set { SetProperty(ref _licenceTerms, value); }
        }
    }
}
