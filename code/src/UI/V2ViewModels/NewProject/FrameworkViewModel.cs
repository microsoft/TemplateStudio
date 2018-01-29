// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using Microsoft.Templates.UI.V2Services;
using Microsoft.Templates.UI.V2ViewModels.Common;

namespace Microsoft.Templates.UI.V2ViewModels.NewProject
{
    public class FrameworkViewModel : SelectableGroup<MetadataInfoViewModel>
    {
        public FrameworkViewModel(Func<bool> isSelectionEnabled)
            : base(isSelectionEnabled)
        {
        }

        public void LoadData(string projectTypeName)
        {
            if (DataService.LoadFrameworks(Items, projectTypeName))
            {
                BaseMainViewModel.BaseInstance.ProcessItem(Items.First());
            }
        }
    }
}
