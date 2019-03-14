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
    public class FrameworkViewModel : SelectableGroup<FrameworkMetaDataViewModel>
    {
        public FrameworkViewModel(Func<bool> isSelectionEnabled, Func<Task> onSelected)
            : base(isSelectionEnabled, onSelected)
        {
        }

        public async Task LoadDataAsync(string projectTypeName, string platform)
        {
            if (DataService.LoadFrameworks(Items, projectTypeName, platform))
            {
                await BaseMainViewModel.BaseInstance.ProcessItemAsync(Items.First());
            }
        }
    }
}
