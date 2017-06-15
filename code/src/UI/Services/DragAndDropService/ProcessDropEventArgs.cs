using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace Microsoft.Templates.UI.Services
{
    public class ProcessDropEventArgs<T> : EventArgs where T : class
    {
        private ObservableCollection<T> _itemsSource;
        private T _dataItem;
        private int _oldIndex;
        private int _newIndex;
        private DragDropEffects _allowedEffects = DragDropEffects.None;
        public DragDropEffects Effects { get; set; }

        internal ProcessDropEventArgs(ObservableCollection<T> itemsSource, T dataItem, int oldIndex, int newIndex, DragDropEffects allowedEffects)
        {
            _itemsSource = itemsSource;
            _dataItem = dataItem;
            _oldIndex = oldIndex;
            _newIndex = newIndex;
            _allowedEffects = allowedEffects;
            Effects = DragDropEffects.None;
        }
        
        public ObservableCollection<T> ItemsSource => this._itemsSource;        
        public T DataItem => this._dataItem;        
        public int OldIndex => this._oldIndex;
        public int NewIndex => this._newIndex;        
        public DragDropEffects AllowedEffects => _allowedEffects;
    }
}
