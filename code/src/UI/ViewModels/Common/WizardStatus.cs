// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;

using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.Controls;
using Microsoft.Templates.UI.Services;
using System.Windows.Input;

namespace Microsoft.Templates.UI.ViewModels.Common
{
    public class WizardStatus : Observable
    {
        private bool _isOverlayBoxVisible;

        public bool IsOverlayBoxVisible
        {
            get => _isOverlayBoxVisible;
            set => SetProperty(ref _isOverlayBoxVisible, value);
        }

        private bool _isLoading;

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        private string _wizardTitle;

        public string WizardTitle
        {
            get => _wizardTitle;
            set => SetProperty(ref _wizardTitle, value);
        }

        private string _wizardVersion;

        public string WizardVersion
        {
            get => _wizardVersion;
            set => SetProperty(ref _wizardVersion, value);
        }

        private string _templatesVersion;

        public string TemplatesVersion
        {
            get => _templatesVersion;
            set => SetProperty(ref _templatesVersion, value);
        }

        private bool _hasOverlayBox = true;

        public bool HasOverlayBox
        {
            get => _hasOverlayBox;
            set => SetProperty(ref _hasOverlayBox, value);
        }

        private Visibility _infoShapeVisibility = Visibility.Collapsed;

        public Visibility InfoShapeVisibility
        {
            get => _infoShapeVisibility;
            set => SetProperty(ref _infoShapeVisibility, value);
        }

        private bool _hasContent;

        public bool HasContent
        {
            get => _hasContent;
            set => SetProperty(ref _hasContent, value);
        }

        private StatusViewModel _status = StatusViewModel.EmptyStatus;

        public StatusViewModel Status
        {
            get => _status;
            private set => SetProperty(ref _status, value);
        }

        private StatusViewModel _overlayStatus = StatusViewModel.EmptyStatus;

        public StatusViewModel OverlayStatus
        {
            get => _overlayStatus;
            private set => SetProperty(ref _overlayStatus, value);
        }

        private bool _newVersionAvailable;

        public bool NewVersionAvailable
        {
            get => _newVersionAvailable;
            set => SetProperty(ref _newVersionAvailable, value);
        }

        private bool _showFinishButton;

        public bool ShowFinishButton
        {
            get => _showFinishButton;
            set => SetProperty(ref _showFinishButton, value);
        }

        public double Width { get; }

        public double Height { get; }

        public WizardStatus()
        {
            IsLoading = true;
            var size = SystemService.Instance.GetMainWindowSize();
            Width = size.width;
            Height = size.height;
        }

        public void SetStatus(StatusViewModel status)
        {
            if (status.Status == StatusType.Empty)
            {
                OverlayStatus = status;
                Status = status;
            }
            else
            {
                if (status.Status == StatusType.Information && IsOverlayBoxVisible)
                {
                    OverlayStatus = status;
                }
                else
                {
                    Status = status;
                }
            }
        }

        public void ClearStatus()
        {
            if (Status.CanBeCleared)
            {
                SetStatus(StatusViewModel.EmptyStatus);
            }
        }

        public void TryHideOverlayBox(MouseButtonEventArgs args)
        {
            if (args == null || args.Source == null)
            {
                return;
            }

            var element = args.Source as FrameworkElement;
            var originalSource = args.OriginalSource as FrameworkElement;
            if (element is OverlayBox)
            {
                return;
            }
            else if (element?.Tag != null && element.Tag?.ToString() == "AllowOverlay")
            {
                return;
            }
            else if (originalSource?.Tag != null && originalSource.Tag?.ToString() == "AllowOverlay")
            {
                return;
            }

            IsOverlayBoxVisible = false;
        }
    }
}
