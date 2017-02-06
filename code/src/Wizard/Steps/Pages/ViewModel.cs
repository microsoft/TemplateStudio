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
using System.Windows.Controls;

namespace Microsoft.Templates.Wizard.Steps.Pages
{
    public class ViewModel : StepViewModel
    {
        public ViewModel(WizardContext context) : base(context)
        {
        }

        public override string PageTitle => Strings.PageTitle;

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

        public override async Task InitializeAsync()
        {
            Templates.Clear();

            var selectedPages = Context.GetState<ViewModel, IEnumerable<GenInfo>>();

            if (selectedPages != null)
            {
                Templates.AddRange(selectedPages.Select(p => new PageViewModel(p)));
            }

            await Task.FromResult(true);
        }

        public override void SaveState()
        {
            var selectedPages = Templates
                                    .Select(t => t.Info)
                                    .ToList();
            selectedPages.ForEach(p => p.Parameters.Add("framework", Context.GetState<FrameworkType.ViewModel, GenInfo>().Parameters["framework"]));
            Context.SetState(this, selectedPages); 
        }

        private void ShowAddPageDialog()
        {
            var dialog = new NewPage.NewPageDialog(Context, Templates.Select(t => t.Name));
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

        protected override Page GetPageInternal()
        {
            return new View();
        }
    }
}
