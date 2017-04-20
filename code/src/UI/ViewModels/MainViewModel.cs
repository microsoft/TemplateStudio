using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.Resources;
using Microsoft.Templates.UI.Services;
using Microsoft.Templates.UI.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Microsoft.Templates.UI.ViewModels
{
    public class MainViewModel : Observable
    {
        private bool _canGoBack;
        public static MainViewModel Current;
        private MainView _mainView;

        private string _status;
        public string Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }

        private string _wizardVersion;
        public string WizardVersion
        {
            get { return _wizardVersion; }
            set { SetProperty(ref _wizardVersion, value); }
        }

        private string _templatesVersion;
        public string TemplatesVersion
        {
            get { return _templatesVersion; }
            set { SetProperty(ref _templatesVersion, value); }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private Visibility _finishContentVisibility = Visibility.Collapsed;
        public Visibility FinishContentVisibility
        {
            get { return _finishContentVisibility; }
            set { SetProperty(ref _finishContentVisibility, value); }
        }

        private Visibility _noFinishContentVisibility = Visibility.Visible;
        public Visibility NoFinishContentVisibility
        {
            get { return _noFinishContentVisibility; }
            set { SetProperty(ref _noFinishContentVisibility, value); }
        }

        private RelayCommand _cancelCommand;
        private RelayCommand _goBackCommand;
        private RelayCommand _nextCommand;
        private RelayCommand _createCommand;

        public RelayCommand CancelCommand => _cancelCommand ?? (_cancelCommand = new RelayCommand(OnCancel));
        public RelayCommand BackCommand => _goBackCommand ?? (_goBackCommand = new RelayCommand(OnGoBack, () => _canGoBack));                
        public RelayCommand NextCommand => _nextCommand ?? (_nextCommand = new RelayCommand(OnNext));
        public RelayCommand CreateCommand => _createCommand ?? (_createCommand = new RelayCommand(OnCreate));        

        public ProjectSetupViewModel ProjectSetup { get; private set; } = new ProjectSetupViewModel();
        public ProjectTemplatesViewModel ProjectTemplates { get; private set; } = new ProjectTemplatesViewModel();

        public MainViewModel(MainView mainView)
        {
            _mainView = mainView;
            Current = this;
        }

        public async Task IniatializeAsync()
        {
            Title = StringRes.ProjectSetupTitle;
            GenContext.ToolBox.Repo.Sync.SyncStatusChanged += Sync_SyncStatusChanged;
            try
            {
                WizardVersion = GetWizardVersion();

                await GenContext.ToolBox.Repo.SynchronizeAsync();
                Status = string.Empty;

                TemplatesVersion = GenContext.ToolBox.Repo.GetTemplatesVersion();
            }
            catch (Exception ex)
            {
                Status = StringRes.ErrorSync;

                await AppHealth.Current.Error.TrackAsync(ex.ToString());
                await AppHealth.Current.Exception.TrackAsync(ex);
            }
        }

        private string GetWizardVersion()
        {
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            var versionInfo = FileVersionInfo.GetVersionInfo(assemblyLocation);

            return versionInfo.FileVersion;
        }

        public void UnsuscribeEventHandlers()
        {
            GenContext.ToolBox.Repo.Sync.SyncStatusChanged -= Sync_SyncStatusChanged;
        }

        private void Sync_SyncStatusChanged(object sender, SyncStatus status)
        {

            Status = GetStatusText(status);

            if (status == SyncStatus.Updated)
            {
                TemplatesVersion = GenContext.ToolBox.Repo.GetTemplatesVersion();

                //mvegaca
                //_context.CanGoForward = true;

                //var step = Steps.First();
                //Navigate(step);
            }


            if (status == SyncStatus.OverVersion)
            {
                _mainView.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show(_mainView, StringRes.StatusOverVersionContent, StringRes.StatusOverVersionTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                });

                //TODO: Review message and behavior.
            }

            if (status == SyncStatus.UnderVersion)
            {
                _mainView.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show(_mainView, StringRes.StatusLowerVersionContent, StringRes.StatusLowerVersionContent, MessageBoxButton.OK, MessageBoxImage.Error);
                    //TODO: Review message and behavior.
                });
            }
        }

        private string GetStatusText(SyncStatus status)
        {
            switch (status)
            {
                case SyncStatus.Updating:
                    return StringRes.StatusUpdating;
                case SyncStatus.Updated:
                    return StringRes.StatusUpdated;
                case SyncStatus.Adquiring:
                    return StringRes.StatusAdquiring;
                case SyncStatus.Adquired:
                    return StringRes.StatusAdquired;
                default:
                    return string.Empty;
            }
        }

        private void OnCancel()
        {
            _mainView.DialogResult = false;
            _mainView.Result = null;
            _mainView.Close();
        }        

        private void OnNext()
        {
            NavigationService.Navigate(new ProjectTemplatesView(ProjectSetup.SelectedFramework.Name));
            _canGoBack = true;
            BackCommand.OnCanExecuteChanged();
            FinishContentVisibility = Visibility.Visible;
            NoFinishContentVisibility = Visibility.Collapsed;
        }

        private void OnGoBack()
        {
            NavigationService.GoBack();
            _canGoBack = false;
            BackCommand.OnCanExecuteChanged();
            FinishContentVisibility = Visibility.Collapsed;
            NoFinishContentVisibility = Visibility.Visible;
        }

        private void OnCreate()
        {
        }
    }
}
