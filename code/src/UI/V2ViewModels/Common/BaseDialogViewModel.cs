// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;
using System.Windows.Input;
using Microsoft.Templates.Core.Mvvm;

namespace Microsoft.Templates.UI.V2ViewModels.Common
{
    public abstract class BaseDialogViewModel : Observable
    {
        private ICommand _finishCommand;

        public Window Window { get; }

        public ICommand FinishCommand => _finishCommand ?? (_finishCommand = new RelayCommand<object>(OnFinish));

        public BaseDialogViewModel(Window window)
        {
            Window = window;
        }

        protected virtual void OnFinish(object parameter)
        {
            Window.Close();
        }
    }
}
