using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Templates.Wizard;
using Microsoft.Templates.Core;
using Microsoft.TemplateEngine.Abstractions;

namespace Microsoft.Templates.Wizard.Dialog
{
    public partial class AddPage : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private readonly ObservableCollection<SelectionOption> pageOptions = new ObservableCollection<SelectionOption>();
        private TemplatesRepository templates;

        public AddPageResult ResultInfo { get; private set; }
        public bool IsOkEnabled
        {
            get
            {
                return (PageNameProp.Trim().Length > 0 && NamespaceProp.Trim().Length > 0 && ResultInfo.PageTemplate != null);
            }
        }

        private string pageNameProp = "Sample";
        public String PageNameProp
        {
            get
            {
                return pageNameProp;
            }
            set
            {
                pageNameProp = value;
                NotifyPropertyChanged("PageNameProp");
            }
        }
        private string namespaceProp = "";
        public String NamespaceProp
        {
            get
            {
                return namespaceProp;
            }
            set
            {
                namespaceProp = value;
                NotifyPropertyChanged("NamespaceProp");
            }
        }
        public AddPage(string projectName, string vsTemplateCategory, string suggestedNamespace, TemplatesRepository templatesRepository)
        {
            InitializeComponent();

            templates = templatesRepository;
            LoadOptions(vsTemplateCategory);
            PageOptions.ItemsSource = pageOptions;
            NamespaceProp = suggestedNamespace;
            SolutionNameTbx.Text = $"Add Page to project {projectName}. Showing Category ({vsTemplateCategory})";
            ResultInfo = new AddPageResult();
        }


        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "UWP Community Templates: Cancel Action", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                DialogResult = false;
                Close();
            }
        }
 
        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            ResultInfo.PageName = PageNameProp;
            ResultInfo.Namespace = namespaceProp;
            DialogResult = true;
            Close();
        }

        private void LoadOptions(string vsTemplateCategory)
        {
            var pageTemplates = templates.GetAll().Where(t => t.GetTemplateType() == TemplateType.Page);
            AddOptions(pageTemplates, vsTemplateCategory, pageOptions, SelectPage);
        }

        private void AddOptions(IEnumerable<ITemplateInfo> sourceFormulas, string categoryFilter, ObservableCollection<SelectionOption> targetCollection, Action<ITemplateInfo> relayCommandAction)
        {
            var wizardOptions = sourceFormulas.Select(t => {
                return new SelectionOption
                {
                    Item = t,
                    Command = new RelayCommand( a=> relayCommandAction(t))
                };
            });

            foreach (SelectionOption option in wizardOptions)
            {
                targetCollection.Add(option);
            }
        }

        private void SelectPage(ITemplateInfo templateInfo)
        {
            ResultInfo.PageTemplate = templateInfo;
            NotifyPropertyChanged("IsOkEnabled");
        }

        private void PageName_TextChanged(object sender, TextChangedEventArgs e)
        {
            NotifyPropertyChanged("IsOkEnabled");
        }

        private void DefaultNamespace_TextChanged(object sender, TextChangedEventArgs e)
        {
            NotifyPropertyChanged("IsOkEnabled");
        }
    }
}
