// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Templates.UI.Converters;
using Microsoft.Templates.UI.Styles;
using Microsoft.Templates.UI.ViewModels.Common;
using Microsoft.Templates.UI.ViewModels.NewItem;

namespace Microsoft.Templates.UI.Views.NewItem
{
    public partial class NewItemMainPage : Page
    {
        public NewItemMainPage()
        {
            this.Resources.MergedDictionaries.Add(AllStylesDictionary.GetMergeDictionary());
            this.Resources.Add("HasItemsVisibilityConverter", new HasItemsVisibilityConverter());
            this.Resources.Add("BoolToVisibilityConverter", new BoolToVisibilityConverter());
            this.Resources.Add("StepVisibilityConverter", new StepVisibilityConverter());

            DataContext = MainViewModel.Instance;
            InitializeComponent();
        }

        private void OnLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            if (stepFrame.Content == null)
            {
                Services.NavigationService.InitializeSecondaryFrame(stepFrame, new TemplateSelectionPage());
            }

            Services.NavigationService.SubscribeEventHandlers();
            WizardNavigation.Current.SubscribeEventHandlers();
        }

        private void OnUnloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Services.NavigationService.UnsubscribeEventHandlers();
            WizardNavigation.Current.UnsubscribeEventHandlers();
        }
    }
}
