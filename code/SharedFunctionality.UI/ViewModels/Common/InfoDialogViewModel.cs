// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Windows.Input;
using Microsoft.Templates.UI.Mvvm;
using Microsoft.Templates.UI.Services;

namespace Microsoft.Templates.UI.ViewModels.Common
{
    public class InfoDialogViewModel : BaseDialogViewModel
    {
        private ICommand _openLinkCommand;

        public UIStylesService StylesService { get; }

        public string Link { get; set; }

        public ICommand OpenWebSiteCommand => _openLinkCommand ?? (_openLinkCommand = new RelayCommand(OnOpenLink));

        public InfoDialogViewModel(string title, string message, string link, BaseStyleValuesProvider provider)
        {
            Title = title;
            Description = message;
            Link = link;
            StylesService = new UIStylesService(provider);
        }

        private void OnOpenLink() => Process.Start(Link);
    }
}
