// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Templates.Core;
using Microsoft.Templates.UI.Controls;
using Microsoft.Templates.UI.Extensions;
using Microsoft.Templates.UI.Mvvm;
using Microsoft.Templates.UI.Services;

namespace Microsoft.Templates.UI.ViewModels.Common
{
    public class WizardNavigation : Observable
    {
        public static WizardNavigation Current { get; private set; }

        private Window _wizardShell;

        private bool _canGoBack = false;
        private bool _canGoForward = true;
        private bool _canFinish;
        private StepData _origStep;
        private StepData _currentStep;

        private RelayCommand _cancelCommand;
        private RelayCommand _goBackCommand;
        private RelayCommand _goForwardCommand;
        private RelayCommand _finishCommand;

        public RelayCommand CancelCommand => _cancelCommand ?? (_cancelCommand = new RelayCommand(Cancel));

        public RelayCommand GoBackCommand => _goBackCommand ?? (_goBackCommand = new RelayCommand(GoBack, CanGoBack));

        public RelayCommand GoForwardCommand => _goForwardCommand ?? (_goForwardCommand = new RelayCommand(GoForward, CanGoForward));

        public RelayCommand FinishCommand => _finishCommand ?? (_finishCommand = new RelayCommand(Finish, CanFinish));

        public ObservableCollection<StepData> Steps { get; }

        public Func<StepData, Task<bool>> IsStepAvailable { get; set; }

        public StepData CurrentStep
        {
            get => _currentStep;
        }

        public event EventHandler OnFinish;

        public event EventHandler<StepData> OnStepUpdated;

        public WizardNavigation(Window wizardShell, IEnumerable<StepData> steps, bool canFinish)
        {
            Current = this;
            _wizardShell = wizardShell;
            _canFinish = canFinish;
            Steps = new ObservableCollection<StepData>(steps);
            SetStepAsync(Steps.First()).FireAndForget();
        }

        private bool CanGoBack() => _canGoBack && !WizardStatus.Current.IsBusy;

        private bool CanGoForward() => _canGoForward && !WizardStatus.Current.IsBusy;

        private bool CanFinish() => _canFinish && !WizardStatus.Current.IsBusy;

        public void Cancel() => _wizardShell.Close();

        private async void GoBack()
        {
            var stepIndex = Steps.IndexOf(CurrentStep);
            await SetStepAsync(Steps.ElementAt(stepIndex - 1));
        }

        private async void GoForward()
        {
            var stepIndex = Steps.IndexOf(CurrentStep);
            await SetStepAsync(Steps.ElementAt(stepIndex + 1));
        }

        private void Finish()
        {
            OnFinish?.Invoke(this, EventArgs.Empty);
            _wizardShell.DialogResult = true;
            _wizardShell.Close();
        }

        public async Task SetStepAsync(StepData newStep, bool navigate = true)
        {
            if (WizardStatus.Current.HasValidationErrors)
            {
                return;
            }

            _origStep = _currentStep;
            if (newStep != _currentStep)
            {
                _currentStep = newStep;
            }

            if (IsStepAvailable != null)
            {
                if (!await IsStepAvailable(newStep))
                {
                    DispatcherService.BeginInvoke(() =>
                    {
                        _currentStep = _origStep;
                        OnPropertyChanged(nameof(CurrentStep));
                    });

                    return;
                }
            }

            OnPropertyChanged(nameof(CurrentStep));
            UpdateStep(navigate);
        }

        private void UpdateStep(bool navigate)
        {
            Steps.UnselectAll();

            // Mark previous steps as compleated
            Steps.Where(s => IsPrevious(s, CurrentStep)).ToList()
                 .ForEach(s => s.Completed = true);

            if (CurrentStep != null)
            {
                CurrentStep.Completed = true;
                CurrentStep.IsSelected = true;
                if (navigate)
                {
                    NavigationService.NavigateSecondaryFrame(CurrentStep.GetPage());
                }
            }

            UpdateBackForward();
            OnStepUpdated?.Invoke(this, CurrentStep);
        }

        private bool IsPrevious(StepData value1, StepData value2)
            => Steps.IndexOf(value1) < Steps.IndexOf(value2);

        private void UpdateBackForward()
        {
            var stepIndex = Steps.IndexOf(CurrentStep);
            _canGoBack = stepIndex > 0;
            _canGoForward = stepIndex < Steps.Count - 1;
        }

        public void SetCanFinish(bool canFinish)
        {
            _canFinish = canFinish;
        }

        public void RefreshStep(object navigatedPage)
        {
            var step = Steps.FirstOrDefault(s => s.Equals(navigatedPage.GetType()));
            if (step != null)
            {
                SetStepAsync(step, false).FireAndForget();
            }
        }

        public void AddNewStep(string stepId, string title, Func<object> getPage)
        {
            var newStep = StepData.MainStep(stepId, (Steps.Count + 1).ToString(), title, getPage);
            var nextStep = Steps.FirstOrDefault(s => s.Id.CompareTo(stepId) > 0);
            if (nextStep != null)
            {
                int position = Steps.IndexOf(nextStep);
                Steps.Insert(position, newStep);
                SetStepsIndex();
            }
            else
            {
                Steps.Add(newStep);
            }
        }

        public async Task RemoveStepAsync(string stepId)
        {
            var stepToRemove = Steps.FirstOrDefault(s => s.Id == stepId);
            if (stepToRemove != null)
            {
                var stepToRemoveIndex = Steps.IndexOf(stepToRemove);
                Steps.Remove(stepToRemove);
                if (stepToRemove.IsSelected)
                {
                    await SetStepAsync(Steps.ElementAt(stepToRemoveIndex - 1));
                }
                else
                {
                    UpdateBackForward();
                }

                SetStepsIndex();
            }
        }

        private void SetStepsIndex()
        {
            for (int index = 0; index < Steps.Count; index++)
            {
                Steps[index].Index = (index + 1).ToString();
            }
        }
    }
}
