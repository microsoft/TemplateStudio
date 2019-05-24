// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows.Controls;
using Microsoft.Templates.UI.ViewModels.NewProject;

namespace Microsoft.Templates.UI.Views.NewProject
{
    public partial class AddPagesPage : Page
    {
        public AddPagesPage()
        {
            DataContext = MainViewModel.Instance.StepsViewModels[Core.TemplateType.Page];
            InitializeComponent();
        }
    }
}
