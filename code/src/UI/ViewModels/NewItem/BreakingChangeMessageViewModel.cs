// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.Windows.Input;
using Microsoft.Templates.Core.Validation;
using Microsoft.Templates.UI.Mvvm;

namespace Microsoft.Templates.UI.ViewModels.NewItem
{
    public class BreakingChangeMessageViewModel
    {
        private ICommand _openLinkCommand;

        public BreakingChangeMessageViewModel(ValidationMessage validationMessage)
        {
            Message = validationMessage?.Message ?? string.Empty;
            Url = validationMessage?.Url ?? string.Empty;
            HyperLinkMessage = validationMessage?.HyperLinkMessage ?? string.Empty;
        }

        public string Message { get; set; }

        public string Url { get; set; }

        public string HyperLinkMessage { get; set; }

        public ICommand OpenLinkCommand => _openLinkCommand ?? (_openLinkCommand = new RelayCommand(() => Process.Start(Url), IsValidUrl));

        private bool IsValidUrl()
        {
            return !string.IsNullOrWhiteSpace(Url) && Uri.IsWellFormedUriString(Url, UriKind.Absolute);
        }
}
}
