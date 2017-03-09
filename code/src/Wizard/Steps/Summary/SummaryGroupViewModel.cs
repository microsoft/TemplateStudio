using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.Wizard.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Microsoft.Templates.Wizard.Steps.Summary
{
    public class SummaryGroupViewModel : Observable
    {
        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ObservableCollection<SummaryItemViewModel> Items { get; } = new ObservableCollection<SummaryItemViewModel>();
    }

    public class SummaryItemViewModel : Observable
    {

        public SummaryItemViewModel(GenInfo genInfo)
        {
            //TODO: CHECK NULLS
            Name = genInfo.Name;
            TemplateName = genInfo.Template.Name;
            Author = genInfo.Template.Author;
        }

        public SummaryItemViewModel()
        {
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private string _templateName;
        public string TemplateName
        {
            get { return _templateName; }
            set { SetProperty(ref _templateName, value); }
        }

        private string _author;
        public string Author
        {
            get { return _author; }
            set { SetProperty(ref _author, value); }
        }        
    }
}
