// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows.Input;
using Microsoft.Templates.UI.Mvvm;

namespace Microsoft.Templates.UI.ViewModels.Common
{
    public abstract class BaseDialogViewModel : Observable
    {
        public BaseDialogViewModel()
        {
            AcceptCommand = new RelayCommand(OnAccept);
            CancelCommand = new RelayCommand(OnCancel);
        }

        public string Title { get; set; }

        public string Description { get; set; }

        public ICommand AcceptCommand { get; private set; }

        public ICommand CancelCommand { get; private set; }

        public DialogResult Result { get; set; } = DialogResult.None;

        public Action CloseAction { get; set; }

        protected virtual void OnAccept()
        {
            Result = DialogResult.Accept;
            CloseAction?.Invoke();
        }

        protected virtual void OnCancel()
        {
            Result = DialogResult.Cancel;
            CloseAction?.Invoke();
        }
    }

    public enum DialogResult
    {
        None,
        Accept,
        Cancel,
    }
}
