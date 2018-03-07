// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Templates.Core.Mvvm;

namespace Microsoft.Templates.VsEmulator
{
    public class Selectable<T> : Observable
        where T : class
    {
        private bool _isSelected;

        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        public T Item { get; }

        public Selectable(T item)
        {
            Item = item;
        }
    }
}
