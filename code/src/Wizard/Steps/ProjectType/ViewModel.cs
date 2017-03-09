using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Wizard.Host;
using Microsoft.Templates.Wizard.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Microsoft.Templates.Wizard.Steps.ProjectType
{
    public class ViewModel : StepViewModel
    {
        private bool _alreadyAccepted;

        public ObservableCollection<ProjectInfoViewModel> ProjectTypes { get; } = new ObservableCollection<ProjectInfoViewModel>();
        public override string PageTitle => Strings.PageTitle;

        public ViewModel(WizardContext context) : base(context)
        {
        }

        private ProjectInfoViewModel _selectedProjectType;
        public ProjectInfoViewModel SelectedProjectType
        {
            get => _selectedProjectType;
            set
            {
                //TODO: REVIEW THIS IMPLEMENTATION

                var originalSelected = _selectedProjectType;

                if (ShouldShowResetMessage(value))
                {
                    if (Context.ResetSelection())
                    {
                        _alreadyAccepted = true;
                        CleanState();
                    }
                    else
                    {
                        //UNDO
                        Application.Current.Dispatcher.BeginInvoke(
                            new Action(() =>
                            {
                                SetProperty(ref _selectedProjectType, originalSelected);
                            }),
                            DispatcherPriority.ContextIdle,
                            null
                        );
                    }
                }
                SetProperty(ref _selectedProjectType, value);
            }
        }

        private bool ShouldShowResetMessage(ProjectInfoViewModel value)
        {
            return !string.IsNullOrEmpty(Context.State.ProjectType) && !Context.State.ProjectType.Equals(value.Name) && !_alreadyAccepted;
        }

        public override async Task InitializeAsync()
        {
            ProjectTypes.Clear();

            var projectTypes = GenContext.ToolBox.Repo.GetAll()
                                                            .Where(t => t.GetTemplateType() == TemplateType.Project && !String.IsNullOrWhiteSpace(t.GetProjectType()))
                                                            .Select(t => t.GetProjectType())
                                                            .Distinct()
                                                            .Select(t => new ProjectInfoViewModel(t, GenContext.ToolBox.Repo.GetProjectTypeInfo(t)))
                                                            .ToList();

            ProjectTypes.AddRange(projectTypes);

            if (string.IsNullOrEmpty(Context.State.ProjectType))
            {
                SelectedProjectType = projectTypes.FirstOrDefault();
            }
            else
            {
                SelectedProjectType = projectTypes.FirstOrDefault(p => p.Name == Context.State.ProjectType);
            }

            //TODO: REVIEW ASYNC
            await Task.FromResult(true);
        }

        public override void SaveState() => Context.State.ProjectType = SelectedProjectType.Name;


        protected override Page GetPageInternal()
        {
            return new View();
        }

        private void CleanState()
        {
            Context.State.Framework = null;
            Context.State.Pages.Clear();
            Context.State.DevFeatures.Clear();
            Context.State.ConsumerFeatures.Clear();
        }
    }
}
