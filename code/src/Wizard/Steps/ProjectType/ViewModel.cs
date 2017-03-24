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

        public ObservableCollection<MetadataInfoViewModel> ProjectTypes { get; } = new ObservableCollection<MetadataInfoViewModel>();
        public override string PageTitle => Strings.PageTitle;

        public ViewModel(WizardContext context) : base(context)
        {
        }

        private MetadataInfoViewModel _selectedProjectType;
        public MetadataInfoViewModel SelectedProjectType
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

        private bool ShouldShowResetMessage(MetadataInfoViewModel value)
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
                                                        .Select(t => new MetadataInfoViewModel(t, GenContext.ToolBox.Repo.GetProjectTypeInfo(t)))
                                                        .ToList();

            ProjectTypes.AddRange(projectTypes.Where(p => !string.IsNullOrEmpty(p.Description)));

            if (string.IsNullOrEmpty(Context.State.ProjectType))
            {
                SelectedProjectType = projectTypes.FirstOrDefault();
            }
            else
            {
                SelectedProjectType = projectTypes.FirstOrDefault(p => p.Name == Context.State.ProjectType);
            }

            await Task.CompletedTask;
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
