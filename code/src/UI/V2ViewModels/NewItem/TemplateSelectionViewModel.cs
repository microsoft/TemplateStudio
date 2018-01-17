// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.ObjectModel;
using Microsoft.Templates.Core;
using Microsoft.Templates.UI.V2Services;
using Microsoft.Templates.UI.V2ViewModels.Common;

namespace Microsoft.Templates.UI.V2ViewModels.NewItem
{
    public class TemplateSelectionViewModel
    {
        public ObservableCollection<TemplateGroupViewModel> Groups { get; } = new ObservableCollection<TemplateGroupViewModel>();

        public TemplateSelectionViewModel()
        {
        }

        public void LoadData(TemplateType templateType, string framework)
        {
            DataService.LoadTemplateGroups(Groups, templateType, framework);
        }
    }
}
