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
using System.Windows.Input;

using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.Wizard.Host;
using Microsoft.Templates.Wizard.ViewModels;
using Microsoft.Templates.Wizard.Steps.Pages;
using Microsoft.TemplateEngine.Abstractions;

namespace Microsoft.Templates.Wizard.Steps.ConsumerFeatures.NewConsumerFeature
{
    public class NewConsumerFeatureViewModel : Observable
    {
        private bool _isValid = true;

        private readonly WizardContext _context;
        private readonly NewConsumerFeatureDialog _dialog;
        private readonly IEnumerable<string> _selectedNames;
        private readonly IEnumerable<PageViewModel> _selectedTemplates;

        public NewConsumerFeatureViewModel(WizardContext context, NewConsumerFeatureDialog newDevFeatureDialog, IEnumerable<PageViewModel> selectedTemplates)
        {
            _context = context;
            _dialog = newDevFeatureDialog;
            _selectedNames = selectedTemplates.Select(t => t.Name);
            _selectedTemplates = selectedTemplates;
        }

        public ICommand OkCommand => new RelayCommand(SaveAndClose, IsValid);
        public ICommand CancelCommand => new RelayCommand(_dialog.Close);

        public ObservableCollection<TemplateViewModel> Templates { get; } = new ObservableCollection<TemplateViewModel>();

        private TemplateViewModel _templateSelected;
        public TemplateViewModel TemplateSelected
        {
            get { return _templateSelected; }
            set
            {
                SetProperty(ref _templateSelected, value);
                if (value != null)
                {
                    ItemName = Naming.Infer(_selectedNames, value.Info.GetDefaultName());
                }
            }
        }

        private string _itemName;
        public string ItemName
        {
            get { return _itemName; }
            set
            {
                SetProperty(ref _itemName, value);

                Validate(value);

                OnPropertyChanged(nameof(OkCommand));
            }
        }

        public async Task InitializeAsync()
        {
            Templates.Clear();

            var consumerFeatTemplates = GenContext.ToolBox.Repo
                                                            .Get(t => t.GetTemplateType() == TemplateType.ConsumerFeature
                                                                && t.GetFrameworkList().Contains(_context.State.Framework)
                                                                && (t.GetMultipleInstance() == true || !IsAlreadyDefined(t)))
                                                            .Select(t => new TemplateViewModel(t, GenContext.ToolBox.Repo.GetDependencies(t)))
                                                            .OrderBy(t => t.Order)
                                                            .ToList();

            Templates.AddRange(consumerFeatTemplates);

            if (Templates.Any())
            {
                TemplateSelected = Templates.FirstOrDefault(); 
            }
            else
            {
                _isValid = false;
                OnPropertyChanged(nameof(OkCommand));
            }

            await Task.CompletedTask;
        }

        private bool IsAlreadyDefined(ITemplateInfo template)
        {
            return _selectedTemplates.Any(t => t.TemplateName == template.Name);
        }

        private void SaveAndClose()
        {            
            _dialog.DialogResult = true;
            _dialog.Result = (ItemName, TemplateSelected.Name);

            _dialog.Close();
        }

        private void HandleValidation(ValidationResult validationResult)
        {
            _isValid = validationResult.IsValid;

            if (!validationResult.IsValid)
            {
                var message = Strings.ResourceManager.GetString($"ValidationError_{validationResult.ErrorType}");
                if (string.IsNullOrWhiteSpace(message))
                {
                    message = "UndefinedError";
                }
                throw new Exception(message);
            }
        }

        private void Validate(string value)
        {
            var validationResult = Naming.Validate(_selectedNames, value);

            HandleValidation(validationResult);
        }

        private bool IsValid() => _isValid;
    }
}
