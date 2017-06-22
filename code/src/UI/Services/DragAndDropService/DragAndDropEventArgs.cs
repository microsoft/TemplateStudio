// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

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
