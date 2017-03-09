using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.Wizard.Host;
using Microsoft.Templates.Wizard.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Microsoft.Templates.Wizard.Steps.Pages.NewPage
{
    public class NewPageViewModel : Observable
    {
        private bool _isValid = true;

        private readonly WizardContext _context;
        private readonly NewPageDialog _dialog;
        private readonly IEnumerable<string> _selectedNames;

        public NewPageViewModel(WizardContext context, NewPageDialog dialog, IEnumerable<string> selectedNames)
        {
            _context = context;
            _dialog = dialog;
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
                    ItemName = Naming.Infer(_selectedNames, value.Name);
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

         //TODO: MAKE THIS METHOD TRULY ASYNC
        public async Task InitializeAsync()
        {
            Templates.Clear();

            var pageTemplates = GenContext.ToolBox.Repo.GetAll()
                                                                .Where(f => f.GetTemplateType() == TemplateType.Page
                                                                    && f.GetFrameworkList().Contains(_context.State.Framework))
                                                                .Select(t => new TemplateViewModel(t, GenContext.ToolBox.Repo.GetDependencies(t)))
                                                                .OrderBy(t => t.Order)
                                                                .ToList();

            Templates.AddRange(pageTemplates);

            TemplateSelected = pageTemplates.FirstOrDefault();

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
