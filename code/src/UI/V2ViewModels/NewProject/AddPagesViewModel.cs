// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.ObjectModel;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.V2Services;
using Microsoft.Templates.UI.V2ViewModels.Common;

namespace Microsoft.Templates.UI.V2ViewModels.NewProject
{
    public class AddPagesViewModel : Observable
    {
        public ObservableCollection<TemplateGroupViewModel> Groups { get; } = new ObservableCollection<TemplateGroupViewModel>();

        public AddPagesViewModel()
        {
        }

        public void LoadData(string frameworkName)
        {
            DataService.LoadTemplateGroups(Groups, TemplateType.Page, frameworkName);
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
