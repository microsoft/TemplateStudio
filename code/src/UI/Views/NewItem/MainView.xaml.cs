// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;

using Microsoft.Templates.Core;
using Microsoft.Templates.UI.Services;
using Microsoft.Templates.UI.ViewModels.NewItem;

namespace Microsoft.Templates.UI.Views.NewItem
{
    public partial class MainView : Window
    {
        public MainViewModel ViewModel { get; }

        public UserSelection Result { get; set; }

        public MainView(TemplateType templateType, string language)
        {
            ViewModel = new MainViewModel(language);
            ViewModel.SetView(this);

            DataContext = ViewModel;

            Loaded += async (sender, e) =>
            {
                await ViewModel.InitializeAsync(templateType);
                NavigationService.Initialize(stepFrame, new NewItemSetupView());
            };

            Unloaded += (sender, e) =>
            {
                ViewModel.UnsuscribeEventHandlers();
            };

            InitializeComponent();
        }

        private void OnPreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ViewModel.WizardStatus.TryHideOverlayBox(e);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!DialogResult.HasValue || !DialogResult.Value)
            {
                NewItemGenController.Instance.CleanupTempGeneration();
            }

            if (Result != null && Result.ItemGenerationType == ItemGenerationType.None)
            {
                Result = null;
            }
        }
    }
}
