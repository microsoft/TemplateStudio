using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Templates.Wizard.Host;
using System.Windows.Input;
using Microsoft.Templates.Wizard.Dialog;
using System.Collections.ObjectModel;
using Microsoft.Templates.Wizard.ViewModels;
using Microsoft.Templates.Core;

namespace Microsoft.Templates.Wizard.Steps.DevFeatures.NewDevFeature
{
    public class NewDevFeatureViewModel : ObservableBase
    {
        private bool _isValid = true;

        private readonly WizardContext _context;
        private readonly NewDevFeatureDialog _dialog;
        private readonly IEnumerable<string> _selectedNames;

        public NewDevFeatureViewModel(WizardContext context, NewDevFeatureDialog newDevFeatureDialog, IEnumerable<string> selectedNames)
        {
            _context = context;
            _dialog = newDevFeatureDialog;
            _selectedNames = selectedNames;
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

                OnPropertyChanged("OkCommand");
            }
        }

        public async Task InitializeAsync()
        {
            Templates.Clear();

            var devFeatTemplates = _context.TemplatesRepository.Get(t => t.GetTemplateType() == TemplateType.DevFeature)
                                                                    .Select(t => new TemplateViewModel(t))
                                                                    .OrderBy(t => t.Order)
                                                                    .ToList();

            Templates.AddRange(devFeatTemplates);

            TemplateSelected = devFeatTemplates.FirstOrDefault();

            await Task.FromResult(true);
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
