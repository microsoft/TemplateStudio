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

using System.Collections.Generic;
using System.Linq;

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Mvvm;
using System.Collections.ObjectModel;

namespace Microsoft.Templates.UI.ViewModels
{
    public class TemplateInfoViewModel : Observable
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _templateName;
        public string TemplateName
        {
            get => _templateName;
            set => SetProperty(ref _templateName, value);
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
            get { return _summary; }
            set { SetProperty(ref _summary, value); }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
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

        private IEnumerable<TemplateLicense> _licenseTerms;
        public IEnumerable<TemplateLicense> LicenseTerms
        {
            get { return _licenseTerms; }
            set { SetProperty(ref _licenseTerms, value); }
        }        

        private string _group;
        public string Group
        {
            get { return _group; }
            set { SetProperty(ref _group, value); }
        }

        private bool _isEnabled;
        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        private string _dependencies;
        public string Dependencies
        {
            get { return _dependencies; }
            set { SetProperty(ref _dependencies, value); }
        }

        public ObservableCollection<DependencyInfoViewModel> DependencyItems { get; } = new ObservableCollection<DependencyInfoViewModel>();        

        private TemplateType _templateType;
        public TemplateType TemplateType
        {
            get { return _templateType; }
            set { SetProperty(ref _templateType, value); }
        }

        public ITemplateInfo Template { get; set; }

        private bool _canChooseItemName;
        public bool CanChooseItemName
        {
            get => _canChooseItemName;
            set => SetProperty(ref _canChooseItemName, value);
        }

        public TemplateInfoViewModel(ITemplateInfo template, IEnumerable<ITemplateInfo> dependencies)
        {
            Author = template.Author;
            CanChooseItemName = template.GetItemNameEditable();
            Description = template.GetRichDescription();
            Group = template.GetGroup();
            Icon = template.GetIcon();
            LicenseTerms = template.GetLicenses();
            MultipleInstances = template.GetMultipleInstance();
            Name = template.Name;
            Order = template.GetOrder();
            Summary = template.Description;
            TemplateType = template.GetTemplateType();
            Template = template;
            Version = template.GetVersion();
            
            if (dependencies != null && dependencies.Any())
            {
                DependencyItems.AddRange(dependencies.Select(d => new DependencyInfoViewModel(new TemplateInfoViewModel(d, GenComposer.GetAllDependencies(d, MainViewModel.Current.ProjectSetup.SelectedFramework.Name)))));

                Dependencies = string.Join(",", dependencies.Select(d => d.Name));
            }
        }
    }
}
 