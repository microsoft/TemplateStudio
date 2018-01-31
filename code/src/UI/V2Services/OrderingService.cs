// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using Microsoft.Templates.UI.V2ViewModels.Common;
using Microsoft.Templates.UI.V2ViewModels.NewProject;

namespace Microsoft.Templates.UI.V2Services
{
    public static class OrderingService
    {
        private static ListView _listView;

        private static ObservableCollection<SavedTemplateViewModel> Pages
        {
            get => MainViewModel.Instance.UserSelection.Pages;
        }

        public static void Initialize(ListView listView)
        {
            _listView = listView;

            var service = new DragAndDropService<SavedTemplateViewModel>(listView, AreCompatibleItems);
            service.ProcessDrop += OnDrop;
        }

        public static void MoveUp(SavedTemplateViewModel item)
        {
            if (Pages.Contains(item) && Pages.First() != item)
            {
                var index = Pages.IndexOf(item);
                MoveItemAndSetFocus(index, index - 1);
            }
        }

        public static void MoveDown(SavedTemplateViewModel item)
        {
            if (Pages.Contains(item) && Pages.Last() != item)
            {
                var index = Pages.IndexOf(item);
                MoveItemAndSetFocus(index, index + 1);
            }
        }

        private static bool AreCompatibleItems(int indexOfItem1, int indexOfItem2)
        {
            return AreCompatibleItems(Pages.ElementAt(indexOfItem1), Pages.ElementAt(indexOfItem2));
        }

        private static bool AreCompatibleItems(SavedTemplateViewModel startItem, SavedTemplateViewModel endItem)
        {
            return startItem.GenGroup == endItem.GenGroup;
        }

        private static void OnDrop(object sender, DragAndDropEventArgs<SavedTemplateViewModel> e)
        {
            if (e.OldIndex > -1)
            {
                MoveItemAndSetFocus(e.OldIndex, e.NewIndex);
            }
        }

        private static void MoveItemAndSetFocus(int oldIndex, int newIndex)
        {
            if (AreCompatibleItems(oldIndex, newIndex))
            {
                Pages.Move(oldIndex, newIndex);
                EventService.Instance.RaiseOnReorderTemplate();

                _listView.UpdateLayout();
                var item = _listView.ItemContainerGenerator.ContainerFromIndex(newIndex) as ListBoxItem;
                item?.Focus();
            }
        }
    }
}
