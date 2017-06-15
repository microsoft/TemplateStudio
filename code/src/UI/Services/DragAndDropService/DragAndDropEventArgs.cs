using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace Microsoft.Templates.UI.Services
{
    public class DragAndDropEventArgs<T> : EventArgs where T : class
    {
        public ObservableCollection<T> Items { get; }
        public T ItemData { get; }
        public int OldIndex { get; }
        public int NewIndex { get; }
        public DragDropEffects Effects { get; }

        public DragAndDropEventArgs(ObservableCollection<T> items, T dataItem, int oldIndex, int newIndex, DragDropEffects effects = DragDropEffects.None)
        {
            Items = items;
            ItemData = dataItem;
            OldIndex = oldIndex;
            NewIndex = newIndex;
            Effects = effects;
        }               
    }
}
