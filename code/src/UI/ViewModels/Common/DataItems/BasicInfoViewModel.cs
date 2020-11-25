// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Microsoft.Templates.UI.Mvvm;
using Microsoft.Templates.UI.Services;
using Microsoft.Templates.UI.Views.Common;

namespace Microsoft.Templates.UI.ViewModels.Common
{
    public abstract class BasicInfoViewModel : Selectable
    {
        private string _title;
        private string _summary;
        private string _description;
        private string _author;
        private string _version;
        private string _icon;
        private int _order;
        private bool _isHidden;
        private string _flag;
        private RelayCommand _detailsCommand;
        private RelayCommand _goBackCommand;

        public string Identity { get; protected set; }

        public string Name { get; protected set; }

        public string DefaultName { get; protected set; }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string Summary
        {
            get => _summary;
            set => SetProperty(ref _summary, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public string Author
        {
            get => _author;
            set => SetProperty(ref _author, value);
        }

        public string Version
        {
            get => _version;
            set => SetProperty(ref _version, value);
        }

        public string Icon
        {
            get => _icon;
            set => SetProperty(ref _icon, value);
        }

        public int Order
        {
            get => _order;
            set => SetProperty(ref _order, value);
        }

        public bool IsHidden
        {
            get => _isHidden;
            set => SetProperty(ref _isHidden, value);
        }

        public string Flag
        {
            get => _flag;
            set => SetProperty(ref _flag, value);
        }

        public IEnumerable<BasicInfoViewModel> Dependencies { get; protected set; }

        public IEnumerable<BasicInfoViewModel> Requirements { get; protected set; }

        public IEnumerable<BasicInfoViewModel> Exclusions { get; protected set; }

        public IEnumerable<string> RequiredSdks { get; protected set; }

        public IEnumerable<string> RequiredDotNetVersion { get; protected set; }

        public IEnumerable<string> RequiredVisualStudioWorkloads { get; protected set; }

        public IEnumerable<LicenseViewModel> Licenses { get; protected set; }

        public RelayCommand DetailsCommand => _detailsCommand ?? (_detailsCommand = new RelayCommand(OnDetails, () => !WizardStatus.Current.IsBusy));

        public RelayCommand GoBackCommand => _goBackCommand ?? (_goBackCommand = new RelayCommand(OnGoBack));

        public virtual bool HasLayout { get; } = false;

        public virtual bool HasFrameworks { get; } = false;

        protected BasicInfoViewModel()
            : base(false)
        {
        }

        private void OnDetails()
        {
            NavigationService.NavigateMainFrame(new TemplateInfoPage(this));
        }

        private void OnGoBack()
        {
            NavigationService.GoBackMainFrame();
        }

        public override bool Equals(object obj)
        {
            var result = false;
            if (obj is BasicInfoViewModel info)
            {
                result = Name.Equals(info.Name);
            }

            return result;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
