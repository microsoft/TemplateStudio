using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.VisualStudio.TemplateWizard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.UI.VisualStudio
{
    class RightClick
    {
        void AddNewPage()
        {
            var userSelection = NewItemGenController.Instance.GetUserSelectionNewItem(TemplateType.Page);

            if (userSelection != null)
            {

                NewItemGenController.Instance.FinishGeneration(userSelection);
                TempFolderAvailable = Visibility.Visible;
                GenContext.ToolBox.Shell.ShowStatusBarMessage("Item created!!!");
            }
        }

        void AddNewFeature()
        {

        }

        void SafeExecute(Action action)
        {
            try
            {

            }
            catch (WizardBackoutException)
            {
                GenContext.ToolBox.Shell.ShowStatusBarMessage("Wizard back out");
            }
            catch (WizardCancelledException)
            {
                GenContext.ToolBox.Shell.ShowStatusBarMessage("Wizard cancelled");
            }
        }
    }
}
