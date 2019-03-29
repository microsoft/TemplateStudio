// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Input;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.UI.Mvvm;
using Microsoft.Templates.UI.Resources;

namespace Microsoft.Templates.UI.ViewModels.Common
{
    public class WizardStatus : Observable
    {
        private string _title;
        private string _versions;
        private bool _isBusy;
        private bool _isNotBusy;
        private bool _hasValidationErrors;
        private bool _isSequentialFlowEnabled;
        private bool _isLoading = true;
        private ICommand _openWebSiteCommand;
        private ICommand _createIssueCommand;

        public static WizardStatus Current { get; private set; }

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

        public bool IsSequentialFlowEnabled
        {
            get => _isSequentialFlowEnabled;
            private set => SetProperty(ref _isSequentialFlowEnabled, value);
        }

        public bool HasValidationErrors
        {
            get => _hasValidationErrors;
            set
            {
                SetProperty(ref _hasValidationErrors, value);
                UpdateIsBusyAsync().FireAndForget();
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                SetProperty(ref _isLoading, value);
                UpdateIsBusyAsync().FireAndForget();
            }
        }

        public ICommand OpenWebSiteCommand => _openWebSiteCommand ?? (_openWebSiteCommand = new RelayCommand(OnOpenWebSite));

        public ICommand CreateIssueCommand => _createIssueCommand ?? (_createIssueCommand = new RelayCommand(OnCreateIssue));

        public WizardStatus()
        {
            Current = this;
            var size = BaseMainViewModel.BaseInstance.SystemService.GetMainWindowSize();
            Width = size.width;
            Height = size.height;
            UpdateIsBusyAsync().FireAndForget();
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

        private async Task UpdateIsBusyAsync()
        {
            IsBusy = IsLoading || HasValidationErrors;
            IsNotBusy = !IsBusy;
            IsSequentialFlowEnabled = await BaseMainViewModel.BaseInstance.IsStepAvailableAsync();
        }

        private void OnOpenWebSite() => Process.Start("https://aka.ms/wts/");

        private void OnCreateIssue()
        {
            var vsInfo = GenContext.ToolBox.Shell.GetVSTelemetryInfo();
            var sb = new StringBuilder();
            sb.AppendLine("### For new Features:");
            sb.AppendLine();
            sb.AppendLine("#### Overview");
            sb.AppendLine();
            sb.AppendLine("What you'd think should be added with scenarios");
            sb.AppendLine();
            sb.AppendLine("#### Links");
            sb.AppendLine();
            sb.AppendLine("### For Bugs:");
            sb.AppendLine();
            sb.AppendLine("#### Repro steps");
            sb.AppendLine();
            sb.AppendLine("What did you do? How can someone else reproduce this?");
            sb.AppendLine();
            sb.AppendLine("#### Expected Behavior");
            sb.AppendLine();
            sb.AppendLine("What was expected to happen");
            sb.AppendLine("(include screenshots if a visual issue)");
            sb.AppendLine();
            sb.AppendLine("#### Actual Behavior");
            sb.AppendLine();
            sb.AppendLine("What really happened");
            sb.AppendLine();
            sb.AppendLine("### System");
            sb.AppendLine();
            if (!string.IsNullOrEmpty(vsInfo.VisualStudioEdition) && !string.IsNullOrEmpty(vsInfo.VisualStudioExeVersion))
            {
                sb.AppendLine($"* **VS Version: {vsInfo.VisualStudioEdition} {vsInfo.VisualStudioExeVersion}**");
            }
            else
            {
                sb.AppendLine($"* **VS Version:**");
            }

            sb.AppendLine($"* **WTS Wizard Version: {GenContext.ToolBox.WizardVersion}**");
            sb.AppendLine($"* **WTS Template Version: {GenContext.ToolBox.TemplatesVersion}**");
            sb.AppendLine($"* **Windows Build: {Environment.OSVersion.Version}**");

            var body = HttpUtility.UrlEncode(sb.ToString());
            Process.Start($"https://github.com/Microsoft/WindowsTemplateStudio/issues/new?body={body}");
        }
    }
}
