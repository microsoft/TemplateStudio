// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.UI.ViewModels.Common;

namespace Microsoft.Templates.UI.Services
{
    public class DialogService
    {
        private static Lazy<DialogService> _instance = new Lazy<DialogService>(() => new DialogService());

        public static DialogService Instance => _instance.Value;

        private DialogService()
        {
        }

        public void ShowError(Exception ex, string message = null)
        {
            AppHealth.Current.Exception.TrackAsync(ex, message).FireAndForget();

            var vm = new ErrorDialogViewModel(ex);
            var error = new Views.Common.ErrorDialog(vm);

            GenContext.ToolBox.Shell.ShowModal(error);
        }
    }
}
