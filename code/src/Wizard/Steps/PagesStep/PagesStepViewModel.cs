using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.Wizard.Host;
using Microsoft.Templates.Wizard.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Wizard.Steps.PagesStep
{
    public class PagesStepViewModel : ObservableBase
    {
        private readonly WizardContext _context;

        public PagesStepViewModel(WizardContext context)
        {
            //TODO: VERIFY NOT NULL
            _context = context;
            _context.SaveState += _context_SaveState;
        }

        private void _context_SaveState(object sender, EventArgs e)
        {
            if (_context.SelectedTemplates.ContainsKey(this.GetType()))
            {
                _context.SelectedTemplates.Remove(this.GetType());
            }

            var genInfo = new GenInfo
            {
                Name = ItemName,
                Template = TemplateSelected.Info
            };
 
            _context.SelectedTemplates.Add(this.GetType(), new GenInfo[] { genInfo });
        }

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
                    _context.CanGoForward = true;
                    ItemName = value.Name;
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
                if (string.IsNullOrWhiteSpace(value))
                {
                    _context.CanGoForward = false;
                }
                else
                {
                    _context.CanGoForward = true;
                }
            }
        }

        //TODO: MAKE THIS METHOD TRULY ASYNC
        public async Task LoadDataAsync()
        {
            Templates.Clear();

            var projectTemplates = _context.TemplatesRepository
                                                    .GetAll()
                                                    .Where(f => f.GetTemplateType() == TemplateType.Page)
                                                    .Select(t => new TemplateViewModel(t))
                                                    .OrderBy(t => t.Order)
                                                    .ToList();

            Templates.AddRange(projectTemplates);

            TemplateSelected = projectTemplates.FirstOrDefault();

            await Task.FromResult(true);
        }
    }
}
