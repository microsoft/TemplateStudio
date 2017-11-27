// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.V2ViewModels.Common;

namespace Microsoft.Templates.UI.V2ViewModels.NewProject
{
    public class ProjectTypeViewModel : Observable
    {
        public ObservableCollection<BasicInfoViewModel> Items { get; } = new ObservableCollection<BasicInfoViewModel>();

        public ProjectTypeViewModel()
        {
        }

        public void LoadData()
        {
            if (!Items.Any())
            {
                Items.Add(new BasicInfoViewModel()
                {
                    Title = "Navigation pane",
                    Description = "Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
                });
                Items.Add(new BasicInfoViewModel()
                {
                    Title = "Empty project",
                    Description = "Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
                });
                Items.Add(new BasicInfoViewModel()
                {
                    Title = "Pivot and tabs",
                    Description = "Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
                });
            }
        }
    }
}
