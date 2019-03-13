// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Templates.UI.Services;
using Microsoft.Templates.UI.ViewModels.Common;

namespace Microsoft.Templates.UI.ViewModels.NewProject
{
    public class ProjectTypeViewModel : SelectableGroup<ProjectTypeMetaDataViewModel>
    {
        public ProjectTypeViewModel(Func<bool> isSelectionEnabled, Func<Task> onSelected)
            : base(isSelectionEnabled, onSelected)
        {
        }

        public async Task LoadDataAsync(string platform)
        {
            if (DataService.LoadProjectTypes(Items, platform))
            {
                await BaseMainViewModel.BaseInstance.ProcessItemAsync(Items.First());
            }
        }
    }
}
