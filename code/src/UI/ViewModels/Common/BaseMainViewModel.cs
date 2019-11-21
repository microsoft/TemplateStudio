// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.UI.Controls;
using Microsoft.Templates.UI.Extensions;
using Microsoft.Templates.UI.Mvvm;
using Microsoft.Templates.UI.Resources;
using Microsoft.Templates.UI.Services;
using Microsoft.Templates.UI.Threading;

namespace Microsoft.Templates.UI.ViewModels.Common
{
    public abstract class BaseMainViewModel : Observable
    {
        public static BaseMainViewModel BaseInstance { get; private set; }

        protected Window MainView { get; private set; }

        protected string Language { get; private set; }

        protected string Platform { get; private set; }

        public WizardStatus WizardStatus { get; }

        public SystemService SystemService { get; }

        public UIStylesService StylesService { get; }

        public WizardNavigation Navigation { get; }

        public BaseMainViewModel(Window mainView, BaseStyleValuesProvider provider, IEnumerable<StepData> steps, bool canFinish = true)
        {
            BaseInstance = this;
            MainView = mainView;
            SystemService = new SystemService();
            WizardStatus = new WizardStatus();
            StylesService = new UIStylesService(provider);
            Navigation = new WizardNavigation(mainView, steps, canFinish);
            ResourcesService.Instance.Initialize(mainView);
        }

        public abstract bool IsSelectionEnabled(MetadataType metadataType);

        public abstract Task ProcessItemAsync(object item);

        protected abstract Task OnTemplatesAvailableAsync();

        public virtual void UnsubscribeEventHandlers()
        {
            GenContext.ToolBox.Repo.Sync.SyncStatusChanged -= OnSyncStatusChanged;
            SystemService.UnsubscribeEventHandlers();
            StylesService.UnsubscribeEventHandlers();
        }

        public virtual async Task InitializeAsync(string platform, string language)
        {
            Platform = platform;
            Language = language;

            // Templates generation is not suported in following cases:
            // - WPF Projects from VisualStudio 2017
            var vsInfo = GenContext.ToolBox.Shell.GetVSTelemetryInfo();
            var version = vsInfo.VisualStudioExeVersion.Split('.').Select(n => int.Parse(n)).ToList();
            if (Platform == Platforms.Wpf && (version[0] < 16 || version[1] < 3))
            {
                WizardStatus.CanNotGenerateProjectsMessage = StringRes.CanNotGenerateWPFProjectsMessage;
                return;
            }

            GenContext.ToolBox.Repo.Sync.SyncStatusChanged += OnSyncStatusChanged;
            SystemService.Initialize();
            try
            {
                await GenContext.ToolBox.Repo.SynchronizeAsync();
            }
            catch (Exception ex)
            {
                await AppHealth.Current.Error.TrackAsync(ex.ToString());
                await AppHealth.Current.Exception.TrackAsync(ex);
            }
        }

        private async void OnSyncStatusChanged(object sender, SyncStatusEventArgs args)
        {
            await SafeThreading.JoinableTaskFactory.SwitchToMainThreadAsync();
            WizardStatus.SetVersions();

            var notification = args.GetNotification();
            if (notification?.Category == Category.TemplatesSync)
            {
                await NotificationsControl.AddOrUpdateNotificationAsync(notification);
            }
            else
            {
                await NotificationsControl.AddNotificationAsync(notification);
            }

            if (args.Status == SyncStatus.NoUpdates || args.Status == SyncStatus.Ready)
            {
                NotificationsControl.RemoveNotification();
            }

            if (args.Status == SyncStatus.Updated || args.Status == SyncStatus.Ready)
            {
                await OnTemplatesAvailableAsync();
            }
        }
    }
}
