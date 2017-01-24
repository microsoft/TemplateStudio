using Microsoft.Templates.Wizard.Host;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Microsoft.Templates.Wizard.Steps
{
    public abstract class StepPage : Page
    {
        protected WizardContext Context { get; }

        public abstract string PageTitle { get; }

        public StepPage()
        {
        }

        public StepPage(WizardContext context)
        {
            Context = context;
        }
    }
}
