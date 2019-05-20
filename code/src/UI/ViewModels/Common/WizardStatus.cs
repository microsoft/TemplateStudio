// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.UI.Mvvm;
using Microsoft.Templates.UI.Resources;
using Microsoft.Templates.UI.Services;

namespace Microsoft.Templates.UI.ViewModels.Common
{
    public class WizardStatus : Observable
    {
        public static WizardStatus Current { get; private set; }

        private string _title;
        private string _versions;
        private bool _isBusy;
        private bool _isNotBusy;
        private bool _hasValidationErrors;
        private bool _isLoading = true;
        private ICommand _openWebSiteCommand;

        public double Width { get; }

        public double Height { get; }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string Versions
        {
            get => _versions;
            set => SetProperty(ref _versions, value);
        }

        public bool IsBusy
        {
            get => _isBusy;
            private set
            {
                SetProperty(ref _isBusy, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public bool IsNotBusy
        {
            get => _isNotBusy;
            private set => SetProperty(ref _isNotBusy, value);
        }

        public bool HasValidationErrors
        {
            get => _hasValidationErrors;
            set
            {
                SetProperty(ref _hasValidationErrors, value);
                UpdateIsBusy();
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                SetProperty(ref _isLoading, value);
                UpdateIsBusy();
            }
        }

        public ICommand OpenWebSiteCommand => _openWebSiteCommand ?? (_openWebSiteCommand = new RelayCommand(OnOpenWebSite));

        public WizardStatus()
        {
            Current = this;
            var size = SystemService.Current.GetMainWindowSize();
            Width = size.width;
            Height = size.height;
            UpdateIsBusy();
            SetVersions();
        }

        public void SetVersions()
        {
            var versionsStringBuilder = new StringBuilder();
            versionsStringBuilder.AppendLine($"{StringRes.ProjectDetailsAboutSectionTemplatesVersion} {GenContext.ToolBox.TemplatesVersion}");
            versionsStringBuilder.Append($"{StringRes.ProjectDetailsAboutSectionWizardVersion} {GenContext.ToolBox.WizardVersion}");
            var versionsText = versionsStringBuilder.ToString();
            if (string.IsNullOrEmpty(Versions) || !Versions.Equals(versionsText))
            {
                Versions = versionsText;
            }
        }

        private void UpdateIsBusy()
        {
            IsBusy = IsLoading || HasValidationErrors;
            IsNotBusy = !IsBusy;
        }

        private void OnOpenWebSite() => Process.Start("https://aka.ms/wts");
    }
}
