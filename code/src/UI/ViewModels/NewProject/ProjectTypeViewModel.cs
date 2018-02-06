// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using Microsoft.Templates.UI.Services;
using Microsoft.Templates.UI.ViewModels.Common;

namespace Microsoft.Templates.UI.ViewModels.NewProject
{
    public class ProjectTypeViewModel : SelectableGroup<MetadataInfoViewModel>
    {
        public ProjectTypeViewModel(Func<bool> isSelectionEnabled)
            : base(isSelectionEnabled)
        {
        }

        public void LoadData()
        {
            if (DataService.LoadProjectTypes(Items))
            {
                BaseMainViewModel.BaseInstance.ProcessItem(Items.First());
            }
        }
    }
}
