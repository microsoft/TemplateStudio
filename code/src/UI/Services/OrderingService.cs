// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using Microsoft.Templates.UI.ViewModels.Common;
using Microsoft.Templates.UI.ViewModels.NewProject;

namespace Microsoft.Templates.UI.Services
{
    public class OrderingService
    {
        private ListView _listView;
        private DragAndDropService<SavedTemplateViewModel> _dragAndDropService;

        private ObservableCollection<SavedTemplateViewModel> Pages
        {
            get => MainViewModel.Instance.UserSelection.Groups.First(g => g.TemplateType == Core.TemplateType.Page).Items;
        }

        public OrderingService(ListView listView)
        {
            _listView = listView;
            if (_listView != null)
            {
                _dragAndDropService = new DragAndDropService<SavedTemplateViewModel>(listView, AreCompatibleItems);
                _dragAndDropService.ProcessDrop += OnDrop;
            }
        }

        public void UnsubscribeEventHandlers()
        {
            if (_dragAndDropService != null)
            {
                _dragAndDropService.UnsubscribeEventHandlers();
                _dragAndDropService.ProcessDrop -= OnDrop;
            }
        }

        public void MoveUp(SavedTemplateViewModel item)
        {
            if (Pages.Contains(item) && Pages.First() != item)
            {
                var index = Pages.IndexOf(item);
                MoveItemAndSetFocus(index, index - 1);
            }
        }

        public void MoveDown(SavedTemplateViewModel item)
        {
            if (Pages.Contains(item) && Pages.Last() != item)
            {
                var index = Pages.IndexOf(item);
                MoveItemAndSetFocus(index, index + 1);
            }
        }

        private bool AreCompatibleItems(int indexOfItem1, int indexOfItem2)
        {
            return AreCompatibleItems(Pages.ElementAt(indexOfItem1), Pages.ElementAt(indexOfItem2));
        }

        private bool AreCompatibleItems(SavedTemplateViewModel startItem, SavedTemplateViewModel endItem)
        {
            return startItem.GenGroup == endItem.GenGroup;
        }

        private void OnDrop(object sender, DragAndDropEventArgs<SavedTemplateViewModel> e)
        {
            if (e.OldIndex > -1)
            {
                MoveItemAndSetFocus(e.OldIndex, e.NewIndex);
            }
        }

        private void MoveItemAndSetFocus(int oldIndex, int newIndex)
        {
            if (AreCompatibleItems(oldIndex, newIndex))
            {
                Pages.Move(oldIndex, newIndex);

                if (_listView != null)
                {
                    _listView.UpdateLayout();
                    var item = _listView.ItemContainerGenerator.ContainerFromIndex(newIndex) as ListBoxItem;
                    item?.Focus();
                }
            }
        }
    }
}
