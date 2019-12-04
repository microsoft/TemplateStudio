// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Templates.UI.ViewModels.Common;
using Microsoft.Templates.UI.ViewModels.NewProject;

namespace Microsoft.Templates.UI.Views.NewProject
{
    public partial class ProjectTypePage : Page
    {
        public ProjectTypePage()
        {
            DataContext = MainViewModel.Instance;
            InitializeComponent();
        }
    }
}
