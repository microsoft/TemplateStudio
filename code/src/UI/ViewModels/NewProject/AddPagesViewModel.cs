// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.ObjectModel;
using Microsoft.Templates.Core;
using Microsoft.Templates.UI.Mvvm;
using Microsoft.Templates.UI.Services;
using Microsoft.Templates.UI.ViewModels.Common;

namespace Microsoft.Templates.UI.ViewModels.NewProject
{
    public class AddPagesViewModel : Observable
    {
        public ObservableCollection<TemplateGroupViewModel> Groups { get; } = new ObservableCollection<TemplateGroupViewModel>();

        public AddPagesViewModel()
        {
        }

        public void LoadData(string platform, string projectTypeName, string frameworkName)
        {
            Groups.Clear();
            DataService.LoadTemplatesGroups(Groups, TemplateType.Page, platform, projectTypeName, frameworkName);
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
