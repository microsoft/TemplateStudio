using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Templates.Wizard.Host;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Microsoft.Templates.Wizard.ViewModels;
using Microsoft.Templates.Core;
using Microsoft.Templates.Wizard.Steps.Pages;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.Core.Gen;

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

                var validationResult = Naming.Validate(_selectedNames, value);

                HandleValidation(validationResult);

                OnPropertyChanged(nameof(OkCommand));
            }
        }

        public async Task InitializeAsync()
        {
            Templates.Clear();

            var consumerFeatTemplates = GenContext.ToolBox.Repo
                                                            .Get(t => t.GetTemplateType() == TemplateType.ConsumerFeature
                                                                && t.GetFrameworkList().Contains(_context.State.Framework))
                                                            .Select(t => new TemplateViewModel(t, GenContext.ToolBox.Repo.GetDependencies(t)))
                                                            .OrderBy(t => t.Order)
                                                            .ToList();

            foreach (var template in consumerFeatTemplates)
            {
                if (template.MultipleInstances == true || !IsAlreadyDefined(template))
                {
                    Templates.Add(template);
                }
            }

            if (Templates.Any())
            {
                TemplateSelected = consumerFeatTemplates.FirstOrDefault(); 
            }
            else
            {
                _isValid = false;
                OnPropertyChanged(nameof(OkCommand));
            }

            await Task.FromResult(true);
        }

        private bool IsAlreadyDefined(TemplateViewModel template)
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

        private bool IsValid() => _isValid;
    }
}
