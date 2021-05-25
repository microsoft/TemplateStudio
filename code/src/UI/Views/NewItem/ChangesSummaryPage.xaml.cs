// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows.Controls;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.UI.ViewModels.NewItem;

namespace Microsoft.Templates.UI.Views.NewItem
{
    public partial class ChangesSummaryPage : Page
    {
        private readonly NewItemGenerationResult _output;

        public ChangesSummaryPage(NewItemGenerationResult output)
        {
            _output = output;
            DataContext = MainViewModel.Instance;

            InitializeComponent();
        }

        private void OnLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            MainViewModel.Instance.ChangesSummary.Initialize(_output);
        }
    }
}
