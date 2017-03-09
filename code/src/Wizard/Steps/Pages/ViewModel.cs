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
using System.Windows.Input;
using System.Windows.Controls;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.Core.Gen;

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

            if (Context.State.Pages == null || Context.State.Pages.Count == 0)
            {
                AddFromLayout();
            }
            else
            {
                var selectedPages = Context.State.Pages
                                    .Select(p => new PageViewModel(p.name, p.templateName));

                Templates.AddRange(selectedPages);
            }

            await Task.FromResult(true);
        }

        public override void SaveState()
        {
            Context.State.Pages.Clear();
            Context.State.Pages.AddRange(Templates.Select(t => (t.Name, t.TemplateName)));
        }

        private void ShowAddPageDialog()
        {
            var dialog = new NewPage.NewPageDialog(Context, Templates.Select(t => t.Name));
            var dialogResult = dialog.ShowDialog();

            if (dialogResult.HasValue && dialogResult.Value)
            {
                Templates.Add(new PageViewModel(dialog.Result.name, dialog.Result.templateName));
            }
        }

        private void RemovePage()
        {
            if (TemplateSelected != null && !TemplateSelected.Readonly)
            {
                Templates.Remove(TemplateSelected);
            }
        }

        private void AddFromLayout()
        {
            var projectTemplate = GenContext.ToolBox.Repo.Find(t => t.GetProjectType() == Context.State.ProjectType && t.GetFrameworkList().Any(f => f == Context.State.Framework));
            var layout = projectTemplate.GetLayout();

            foreach (var item in layout)
            {
                var template = GenContext.ToolBox.Repo.Find(t => t.Identity == item.templateIdentity);
                if (template != null && template.GetTemplateType() == TemplateType.Page)
                {
                    Templates.Add(new PageViewModel(item.name, template.Name, item.@readonly));
                }
            }
        }

        protected override Page GetPageInternal()
        {
            return new View();
        }
    }
}
