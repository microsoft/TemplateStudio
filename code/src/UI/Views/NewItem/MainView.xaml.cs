// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Windows;
using System.Windows.Input;
using Microsoft.Templates.Core;
using Microsoft.Templates.UI.Services;
using Microsoft.Templates.UI.ViewModels.NewItem;

namespace Microsoft.Templates.UI.Views.NewItem
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainViewModel ViewModel { get; }
        public UserSelection Result { get; set; }

        public MainView(TemplateType templateType)
        {
            ViewModel = new MainViewModel(this);

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
            var element = e.Source as FrameworkElement;
            ViewModel.TryHideOverlayBox(element);
        }

        private void OnKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }
    }
}
