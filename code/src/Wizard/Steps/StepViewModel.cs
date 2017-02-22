using Microsoft.Templates.Core;
using Microsoft.Templates.Wizard.Host;
using Microsoft.Templates.Wizard.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Microsoft.Templates.Wizard.Steps
{
    public abstract class StepViewModel : ObservableBase
    {
        protected WizardContext Context { get; }

        public abstract string PageTitle { get; }

        public StepViewModel(WizardContext context)
        {
            Context = context;
            Context.CanGoForward = true;
        }

        //TODO: MAKE THIS METHOD TRULY ASYNC
        public abstract Task InitializeAsync();
        public abstract void SaveState();
        public abstract void CleanState();

        protected abstract Page GetPageInternal();

        public Page GetPage()
        {
            var page = GetPageInternal();

            page.DataContext = this;
            page.Loaded += async (sender, e) =>
            {
                await InitializeAsync();
            };

            return page;
        }

        public GenInfo GetFeature(string featureName, string name = null)
        {
            var template = Context.TemplatesRepository.GetAll().FirstOrDefault(t => t.Name == featureName);
            if (template != null)
            {
                var genInfo = new GenInfo() { Template = template };
                if (!string.IsNullOrEmpty(name))
                {
                    genInfo.Name = name;
                }
                else
                {
                    genInfo.Name = template.Name;
                }
                return genInfo;
            }
            return null;
        }
    }
}
