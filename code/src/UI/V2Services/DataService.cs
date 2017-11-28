using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.UI.V2ViewModels.Common;

namespace Microsoft.Templates.UI.V2Services
{
    public static class DataService
    {
        public static bool LoadProjectTypes(ObservableCollection<BasicInfoViewModel> projectTypes)
        {
            projectTypes.Clear();
            if (GenContext.ToolBox.Repo.GetProjectTypes().Any())
            {
                var data = GenContext.ToolBox.Repo.GetProjectTypes().Select(m => new BasicInfoViewModel(m)).ToList();

                foreach (var projectType in data.Where(p => !string.IsNullOrEmpty(p.Description)))
                {
                    projectTypes.Add(projectType);
                }

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
