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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Wizard.Host;
using Microsoft.Templates.Wizard.ViewModels;

namespace Microsoft.Templates.Wizard.Steps.Framework
{
    public class ViewModel : StepViewModel
    {
        private bool _alreadyAccepted;

        public ObservableCollection<MetadataInfoViewModel> Frameworks { get; } = new ObservableCollection<MetadataInfoViewModel>();
        public override string PageTitle => Strings.PageTitle;

        public ViewModel(WizardContext context) : base(context)
        {
        }

        private MetadataInfoViewModel _selectedFramework;
        public MetadataInfoViewModel SelectedFramework
        {
            get => _selectedFramework;
            set
            {
                //TODO: REVIEW THIS IMPLEMENTATION

                var originalSelected = _selectedFramework;

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
                                SetProperty(ref _selectedFramework, originalSelected);
                            }),
                            DispatcherPriority.ContextIdle,
                            null
                        );
                    }
                }
                SetProperty(ref _selectedFramework, value);
            }
        }

        public override async Task InitializeAsync()
        {
            Frameworks.Clear();

            foreach (var fx in GetSupportedFx(Context.State.ProjectType))
            {
                var pi = GenContext.ToolBox.Repo.GetFrameworkTypeInfo(fx);
                if (pi != null)
                {
                    Frameworks.Add(new MetadataInfoViewModel(fx, pi));
                }
            }

            if (string.IsNullOrEmpty(Context.State.Framework))
            {
                SelectedFramework = Frameworks.FirstOrDefault();
            }
            else
            {
                SelectedFramework = Frameworks.FirstOrDefault(f => f.Name == Context.State.Framework);
            }

            await Task.CompletedTask;
        }

        public override void SaveState() => Context.State.Framework = SelectedFramework.Name;

        protected override Page GetPageInternal()
        {
            return new View();
        }

        private IEnumerable<string> GetSupportedFx(string projectType)
        {
            return GenContext.ToolBox.Repo.GetAll()
                                                .Where(t => t.GetProjectType() == projectType)
                                                .SelectMany(t => t.GetFrameworkList())
                                                .Distinct();
        }

        private bool ShouldShowResetMessage(MetadataInfoViewModel value)
        {
            return !string.IsNullOrEmpty(Context.State.Framework) && !Context.State.Framework.Equals(value.Name) && !_alreadyAccepted;
        }

        private void CleanState()
        {
            Context.State.Pages.Clear();
            Context.State.DevFeatures.Clear();
            Context.State.ConsumerFeatures.Clear();
        }
    }
}
