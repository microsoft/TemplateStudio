using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Microsoft.Templates.UI.ViewModels
{
    public class TemplateInfoViewModel : Observable
    {
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                SetProperty(ref _name, value);
                OnPropertyChanged(nameof(DisplayText));
            }
        }

        private string _templateName;
        public string TemplateName
        {
            get => _templateName;
            set
            {
                SetProperty(ref _templateName, value);
                OnPropertyChanged(nameof(DisplayText));
            }
        }

        public string DisplayText
        {
            get
            {
                if (HasDefaultName)
                {
                    return Name;
                }
                else
                {
                    return $"{Name} [{TemplateName}]";
                }
            }
        }

        private string _author;
        public string Author
        {
            get => _author;
            set => SetProperty(ref _author, value);
        }

        private bool _isEnabled;
        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        private bool _hasDefaultName;
        public bool HasDefaultName
        {
            get => _hasDefaultName;
            set => SetProperty(ref _hasDefaultName, value);
        }
    }
}
