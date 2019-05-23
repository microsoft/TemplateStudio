using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Templates.Core;
using Microsoft.Templates.UI.Services;
using Microsoft.Templates.UI.ViewModels.Common;

namespace Microsoft.Templates.UI.ViewModels.NewProject
{
    public class AddTestingViewModel
    {
        public ObservableCollection<TemplateGroupViewModel> Groups { get; } = new ObservableCollection<TemplateGroupViewModel>();

        public AddTestingViewModel()
        {
        }

        public void LoadData(string platform, string projectTypeName, string frameworkName)
        {
            Groups.Clear();
            DataService.LoadTemplatesGroups(Groups, TemplateType.Feature, platform, projectTypeName, frameworkName);
        }

        public void ResetUserSelection()
        {
            foreach (var group in Groups)
            {
                foreach (var template in group.Items)
                {
                    template.ResetTemplateCount();
                }
            }
        }
    }
}
