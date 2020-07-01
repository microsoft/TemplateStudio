// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using Microsoft.Templates.UI.Controls;
using Microsoft.Templates.UI.Resources;
using Microsoft.Templates.UI.ViewModels.Common;

namespace Microsoft.Templates.UI.Styles
{
    public partial class ListView
    {
        private async void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsOnDetailsLink(e.OriginalSource as TextBlock))
            {
                return;
            }

            if (sender is ListViewItem item)
            {
                await SelectByItemAsync(item.Content);
                e.Handled = true;
            }
        }

        private async void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.OriginalSource is Hyperlink)
            {
                return;
            }

            if (sender is System.Windows.Controls.ListView listView && e.Key == Key.Enter)
            {
                await SelectByItemAsync(listView.SelectedItem);
                e.Handled = true;
            }
        }

        private async Task SelectItemAsync(BasicInfoViewModel item)
        {
            switch (item)
            {
                case ProjectTypeMetaDataViewModel projectTypeMeta:
                    if (!BaseMainViewModel.BaseInstance.IsSelectionEnabled(projectTypeMeta.MetadataType))
                    {
                        return;
                    }

                    break;
                case FrameworkMetaDataViewModel frameworkMetadata:
                    if (!BaseMainViewModel.BaseInstance.IsSelectionEnabled(frameworkMetadata.MetadataType))
                    {
                        return;
                    }

                    break;
                default:
                    break;
            }

            await BaseMainViewModel.BaseInstance.ProcessItemAsync(item);
        }

        private async Task SelectStepAsync(StepData step) => await BaseMainViewModel.BaseInstance.Navigation.SetStepAsync(step);

        private void SelectFile(NewItemFileViewModel file) => ViewModels.NewItem.MainViewModel.Instance.ChangesSummary.SelectFile(file);

        private bool IsOnDetailsLink(TextBlock textBlock)
        {
            if (textBlock != null)
            {
                if (textBlock.Text == StringRes.ButtonDetails)
                {
                    return true;
                }
            }

            return false;
        }

        private void OnLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                var command = textBox.Tag as ICommand;
                command?.Execute(e);
            }
        }

        private async Task SelectByItemAsync(object itemType)
        {
            switch (itemType)
            {
                case BasicInfoViewModel info:
                    await SelectItemAsync(info);
                    break;
                case StepData step:
                    await SelectStepAsync(step);
                    break;
                case NewItemFileViewModel file:
                    SelectFile(file);
                    break;
                default:
                    break;
            }
        }
    }
}
