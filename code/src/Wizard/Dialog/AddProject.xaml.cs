using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Microsoft.Templates.Wizard;
using Microsoft.Templates.Core;
using Microsoft.TemplateEngine.Abstractions;

namespace Microsoft.Templates.Wizard.Dialog
{
    internal class  SelectionOption
    {
        public ITemplateInfo Item { get; set; }
        public ICommand Command { get; set; }
    }

    public partial class AddNewProject : Window, INotifyPropertyChanged
    {
        private readonly ObservableCollection<SelectionOption> projectOptions = new ObservableCollection<SelectionOption>();

        private TemplatesRepository templates;

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public AddProjectResult ResultInfo { get; private set; }

        AddProjectSteps initialStep;
        AddProjectSteps _currentStep;
        public AddProjectSteps CurrentStep {
            get
            {
                return _currentStep;
            }
            set
            {
                _currentStep = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("PrevButtonEnabled");
                NotifyPropertyChanged("NextButtonLabel");
                NotifyPropertyChanged("ProjectsVisibilty");
                NotifyPropertyChanged("Summary");
            }
        }

        public bool PrevButtonEnabled
        {
            get
            {
                return CurrentStep != initialStep;
            }
        }
        public string NextButtonLabel
        {
            get
            {
                return CurrentStep == AddProjectSteps.ShowSummary ? "Finish" : "Next >";
            }
        }
        public Visibility ProjectsVisibilty
        {
            get
            {
                return CurrentStep == AddProjectSteps.SelectProject ? Visibility.Visible : Visibility.Hidden;
            }
        }


        public string Summary
        {
            get
            {
                if (CurrentStep == AddProjectSteps.ShowSummary)
                {
                    SummaryInfo.Visibility = Visibility.Visible;
                    return ResultInfo.Summary();
                }
                else
                {
                    SummaryInfo.Visibility = Visibility.Hidden;
                    return "";
                }
            }   
        }

        public AddNewProject(string targetSolutionName, string vsTemplateCategory, TemplatesRepository templatesRepository)
            : this(targetSolutionName,vsTemplateCategory,templatesRepository,AddProjectSteps.SelectProject)
        {
        }

        public AddNewProject(string targetSolutionName, string vsTemplateCategory, TemplatesRepository templatesRepository, AddProjectSteps initial)
        {
            InitializeComponent();
            templates = templatesRepository;
            LoadOptions(vsTemplateCategory);

            initialStep = initial;
            CurrentStep = initial;

            if (initial == AddProjectSteps.SelectProject)
            {
                SolutionNameTbx.Text = $"Select the App type you want to add to {targetSolutionName}";
            }
            else
            {
                SolutionNameTbx.Text = $"Add Feature to the project {targetSolutionName}. Showing Category ({vsTemplateCategory})";
            }
            ProjectOptions.ItemsSource = projectOptions;

            ResultInfo = new AddProjectResult();
        }


        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "UWP Community Templates: Cancel Action", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                DialogResult = false;
                Close();
            }
        }
 
        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentStep == AddProjectSteps.ShowSummary)
            {
                DialogResult = true;
                Close();
            }
            else
            {
                CurrentStep = CurrentStep.Next();
            }
        }

        private void PrevBtn_Click(object sender, RoutedEventArgs e)
        {
            CurrentStep = CurrentStep.Previous();
        }

        private void LoadOptions(string vsTemplateCategory)
        {
            var projectFormulas = templates.GetAll().Where(f => f.GetTemplateType() == TemplateType.Project);
            AddOptions(projectFormulas, vsTemplateCategory, projectOptions, SelectProject);
        }

        private void AddOptions(IEnumerable<ITemplateInfo> sourceTemplates, string categoryFilter, ObservableCollection<SelectionOption> targetCollection, Action<ITemplateInfo> relayCommandAction)
        {
            var options = sourceTemplates.Select(t => {
                return new SelectionOption
                {
                    Item = t,
                    Command = new RelayCommand( a=> relayCommandAction(t))
                };
            });

            foreach (SelectionOption option in options)
            {
                targetCollection.Add(option);
            }
        }

        private void SelectProject(ITemplateInfo templateInfo)
        {
            ResultInfo.ProjectTemplate = templateInfo; 
        }
        private void SelectDevFeature(ITemplateInfo templateInfo)
        {
            SwitchInclusion(ResultInfo.Features.Developer, templateInfo);
        }

        private void SelectCustomerFeature(ITemplateInfo templateInfo)
        {
            SwitchInclusion(ResultInfo.Features.Customer, templateInfo);
        }

        private void SwitchInclusion(List<ITemplateInfo> targetCol, ITemplateInfo item)
        {
            var existing = targetCol.Where(t => t.Name == item.Name).FirstOrDefault();
            if (existing != null)
            {
                targetCol.Remove(existing);
            }
            else
            {
                targetCol.Add(item);
            }
        }
    }
}
