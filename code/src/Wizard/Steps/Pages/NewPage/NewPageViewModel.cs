using Microsoft.Templates.Core;
using Microsoft.Templates.Wizard.Dialog;
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
    public class NewPageViewModel : ObservableBase
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

                OnPropertyChanged("OkCommand");
            }
        }

         //TODO: MAKE THIS METHOD TRULY ASYNC
        public async Task InitializeAsync()
        {
            Templates.Clear();

            var selectedFrameword = GetSelectedFramework();
            var projectTemplates = _context.TemplatesRepository.GetAll()
                                                                    .Where(f => f.GetTemplateType() == TemplateType.Page
                                                                        && f.GetFramework() == selectedFrameword)
                                                                    .Select(t => new TemplateViewModel(t))
                                                                    .OrderBy(t => t.Order)
                                                                    .ToList();

            Templates.AddRange(projectTemplates);

            TemplateSelected = projectTemplates.FirstOrDefault();

            await Task.FromResult(true);
        }

        private void SaveAndClose()
        {
            var genInfo = new GenInfo
            {
                Name = ItemName,
                Template = TemplateSelected.Info
            };

            _dialog.DialogResult = true;
            _dialog.Result = genInfo;

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

        private string GetSelectedFramework()
        {
            var selectedProject = _context.GetState<Steps.FrameworkType.ViewModel, GenInfo>();
            if (selectedProject == null)
            {
                throw new ArgumentException("No way to show the page templates, there is no project template selected");
            }
            return selectedProject.Template.GetFramework();
        }
    }
}
