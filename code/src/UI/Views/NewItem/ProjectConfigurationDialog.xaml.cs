// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;
using Microsoft.Templates.UI.ViewModels.NewItem;

namespace Microsoft.Templates.UI.Views.Common
{
    public partial class ProjectConfigurationDialog : Window
    {
        public ProjectConfigurationDialog(ProjectConfigurationViewModel vm)
        {
            DataContext = vm;
            vm.CloseAction = () => Close();
            vm.Initialize();
            InitializeComponent();
        }
    }
}
