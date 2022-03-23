// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Templates.UI.Converters;
using Microsoft.Templates.UI.Styles;
using Microsoft.Templates.UI.ViewModels.NewItem;

namespace Microsoft.Templates.UI.Views.NewItem
{
    public partial class TemplateSelectionPage : Page
    {
        public TemplateSelectionPage()
        {
            Resources.MergedDictionaries.Add(AllStylesDictionary.GetMergeDictionary());
            Resources.Add("HasItemsVisibilityConverter", new HasItemsVisibilityConverter());
            Resources.Add("BoolToVisibilityConverter", new BoolToVisibilityConverter());
            Resources.Add("SavedTemplateGridConverter", new SavedTemplateGridConverter());

            DataContext = MainViewModel.Instance;
            InitializeComponent();
        }

        private void OnLostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                var command = textBox.Tag as ICommand;
                command?.Execute(e);
            }
        }
    }
}
