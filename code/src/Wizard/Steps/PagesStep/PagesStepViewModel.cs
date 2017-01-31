using Microsoft.TemplateEngine.Abstractions;
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
using System.Windows.Input;

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

        public ICommand AddPageCommand => new RelayCommand(ShowAddPageDialog);
        public ICommand RemovePageCommand => new RelayCommand(RemovePage);

        public ObservableCollection<PageViewModel> Templates { get; } = new ObservableCollection<PageViewModel>();

        private PageViewModel _templateSelected;
        public PageViewModel TemplateSelected
        {
            get { return _templateSelected; }
            set
            {
                SetProperty(ref _templateSelected, value);
            }
        }


        //TODO: MAKE THIS METHOD TRULY ASYNC
        public async Task LoadDataAsync()
        {
            Templates.Clear();

            var selectedTemplates = _context.SelectedTemplates
                                                    .Where(t => t.Key == this.GetType())
                                                    .SelectMany(t => t.Value)
                                                    .Select(g => new PageViewModel(g))
                                                    .ToList();

            Templates.AddRange(selectedTemplates);


            await Task.FromResult(true);
        }

        private void ShowAddPageDialog()
        {
            var dialog = new PagesTemplatesDialog(_context);
            var dialogResult = dialog.ShowDialog();

            if (dialogResult.HasValue && dialogResult.Value && dialog.Result != null)
            {
                Templates.Add(new PageViewModel(dialog.Result));
            }

        }

        private void RemovePage()
        {
            if (TemplateSelected != null)
            {
                Templates.Remove(TemplateSelected);
            }
        }

        private void _context_SaveState(object sender, EventArgs e)
        {
            if (!_context.SelectedTemplates.ContainsKey(this.GetType()))
            {
                _context.SelectedTemplates.Add(this.GetType(), new List<GenInfo>());
            }
            else
            {
                _context.SelectedTemplates[this.GetType()].Clear();
            }

            _context.SelectedTemplates[this.GetType()].AddRange(Templates.Select(t => t.Info));
        }
    }
}
