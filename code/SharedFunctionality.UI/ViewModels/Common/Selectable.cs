// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Templates.UI.Mvvm;

namespace Microsoft.Templates.UI.ViewModels.Common
{
    public class Selectable : Observable
    {
        private bool _isSelected;

        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        public Selectable(bool isSelected)
        {
            _isSelected = isSelected;
        }
    }
}
