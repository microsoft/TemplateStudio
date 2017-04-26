using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.Controls;
using Microsoft.Templates.UI.Resources;
using Microsoft.Templates.UI.Services;
using Microsoft.Templates.UI.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private bool _canGoForward;
        public static MainViewModel Current;
        public MainView MainView;

        private StatusViewModel _status = StatusControl.EmptyStatus;
        public StatusViewModel Status
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

        private Visibility _infoShapeVisibility = Visibility.Collapsed;
        public Visibility InfoShapeVisibility
        {
            get { return _infoShapeVisibility; }
            set { SetProperty(ref _infoShapeVisibility, value); }
        }

        private Visibility _loadingContentVisibility = Visibility.Visible;
        public Visibility LoadingContentVisibility
        {
            get { return _loadingContentVisibility; }
            set { SetProperty(ref _loadingContentVisibility, value); }
        }

        private Visibility _loadedContentVisibility = Visibility.Collapsed;
        public Visibility LoadedContentVisibility
        {
            get { return _loadedContentVisibility; }
            set { SetProperty(ref _loadedContentVisibility, value); }
        }

        private Visibility _noContentVisibility = Visibility.Collapsed;
        public Visibility NoContentVisibility
        {
            get { return _noContentVisibility; }
            set { SetProperty(ref _noContentVisibility, value); }
        }

        private Visibility _createButtonVisibility = Visibility.Collapsed;
        public Visibility CreateButtonVisibility
        {
            get { return _createButtonVisibility; }
            set { SetProperty(ref _createButtonVisibility, value); }
        }

        private Visibility _nextButtonVisibility = Visibility.Visible;
        public Visibility NextButtonVisibility
        {
            get { return _nextButtonVisibility; }
            set { SetProperty(ref _nextButtonVisibility, value); }
        }

        private Visibility _wizardInfoVisibility = Visibility.Collapsed;
        public Visibility WizardInfoVisibility
        {
            get { return _wizardInfoVisibility; }
            set { SetProperty(ref _wizardInfoVisibility, value); }
        }

        private RelayCommand _cancelCommand;
        private RelayCommand _goBackCommand;
        private RelayCommand _nextCommand;
        private RelayCommand _createCommand;

        public RelayCommand CancelCommand => _cancelCommand ?? (_cancelCommand = new RelayCommand(OnCancel));
        public RelayCommand BackCommand => _goBackCommand ?? (_goBackCommand = new RelayCommand(OnGoBack, () => _canGoBack));                
        public RelayCommand NextCommand => _nextCommand ?? (_nextCommand = new RelayCommand(OnNext, () => _canGoForward));
        public RelayCommand CreateCommand => _createCommand ?? (_createCommand = new RelayCommand(OnCreate));        

        public ProjectSetupViewModel ProjectSetup { get; private set; } = new ProjectSetupViewModel();
        public ProjectTemplatesViewModel ProjectTemplates { get; private set; } = new ProjectTemplatesViewModel();

        public ObservableCollection<SummaryLicenceViewModel> SummaryLicences { get; } = new ObservableCollection<SummaryLicenceViewModel>();

        public MainViewModel(MainView mainView)
        {
            MainView = mainView;
            Current = this;
        }

        public async Task IniatializeAsync()
        {            
            GenContext.ToolBox.Repo.Sync.SyncStatusChanged += Sync_SyncStatusChanged;
            try
            {
                await GenContext.ToolBox.Repo.SynchronizeAsync();

                TemplatesVersion = GenContext.ToolBox.TemplatesVersion;
                WizardVersion = GenContext.ToolBox.WizardVersion;
            }
            catch (Exception ex)
            {
                Status = new StatusViewModel(StatusType.Information, StringRes.ErrorSync);

                await AppHealth.Current.Error.TrackAsync(ex.ToString());
                await AppHealth.Current.Exception.TrackAsync(ex);
            }
            finally
            {
                MainViewModel.Current.LoadingContentVisibility = Visibility.Collapsed;                
            }
        }

        public void AlertProjectSetupChanged()
        {
            if (CheckProjectSetupChanged())
            {
                Status = new StatusViewModel(StatusType.Warning, string.Format(StringRes.ResetSelection, ProjectTemplates.ContextProjectType.DisplayName, ProjectTemplates.ContextFramework.DisplayName));
            }    
            else
            {
                Status = StatusControl.EmptyStatus;
            }   
        }

        public void UnsuscribeEventHandlers()
        {
            GenContext.ToolBox.Repo.Sync.SyncStatusChanged -= Sync_SyncStatusChanged;
        }

        public void RebuildLicenses()
        {
            var userSelection = CreateUserSelection();
            var genItems = GenComposer.Compose(userSelection);

            var genLicences = genItems
                                .SelectMany(s => s.Template.GetLicences())
                                .Distinct(new TemplateLicenseEqualityComparer())
                                .ToList();

            SyncLicences(genLicences);
        }

        private void Sync_SyncStatusChanged(object sender, SyncStatus status)
        {

            Status = new StatusViewModel(StatusType.Information, GetStatusText(status));

            if (status == SyncStatus.Updated)
            {
                TemplatesVersion = GenContext.ToolBox.Repo.TemplatesVersion;
                Status = StatusControl.EmptyStatus;

                _canGoForward = true;
                NextCommand.OnCanExecuteChanged();
            }

            if (status == SyncStatus.OverVersion)
            {
                MainView.Dispatcher.Invoke(() =>
                {
                    Status = new StatusViewModel(StatusType.Warning, StringRes.StatusOverVersionContent);
                });
            }

            if (status == SyncStatus.OverVersionNoContent)
            {
                MainView.Dispatcher.Invoke(() =>
                {
                    Status = new StatusViewModel(StatusType.Error, StringRes.StatusOverVersionNoContent);
                    _canGoForward = false;
                    NextCommand.OnCanExecuteChanged();
                });
            }

            if (status == SyncStatus.UnderVersion)
            {
                MainView.Dispatcher.Invoke(() =>
                {
                    Status = new StatusViewModel(StatusType.Error, StringRes.StatusLowerVersionContent);
                    _canGoForward = false;
                    NextCommand.OnCanExecuteChanged();
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
            MainView.DialogResult = false;
            MainView.Result = null;
            MainView.Close();
        }        

        private void OnNext()
        {
           if (CheckProjectSetupChanged())
            {
                ProjectTemplates.ResetSelection();
                Status = StatusControl.EmptyStatus;
            }
            
            NavigationService.Navigate(new ProjectTemplatesView());
            _canGoBack = true;
            BackCommand.OnCanExecuteChanged();
            CreateButtonVisibility = Visibility.Visible;
            NextButtonVisibility = Visibility.Collapsed;
        }

        private void OnGoBack()
        {
            NavigationService.GoBack();
            _canGoBack = false;
            BackCommand.OnCanExecuteChanged();
            CreateButtonVisibility = Visibility.Collapsed;
            NextButtonVisibility = Visibility.Visible;
        }

        private bool CheckProjectSetupChanged()
        {
            if (ProjectTemplates.SavedTemplates != null && ProjectTemplates.SavedTemplates.Count != 0)
            {
                if (ProjectTemplates.ContextFramework.Name != ProjectSetup.SelectedFramework.Name || 
                    ProjectTemplates.ContextProjectType.Name != ProjectSetup.SelectedProjectType.Name)
                {
                    return true;
                }
            }
            return false;
        }

        private void OnCreate()
        {
            var userSelection = CreateUserSelection();

            MainView.DialogResult = true;
            MainView.Result = userSelection;
            MainView.Close();
        }

        private UserSelection CreateUserSelection()
        {
            var userSelection = new UserSelection()
            {
                ProjectType = ProjectSetup.SelectedProjectType?.Name,
                Framework = ProjectSetup.SelectedFramework?.Name
            };
            userSelection.Pages.AddRange(ProjectTemplates.SavedPages);
            userSelection.Features.AddRange(ProjectTemplates.SavedFeatures);
            return userSelection;
        }

        private void SyncLicences(IEnumerable<TemplateLicense> licenses)
        {
            var toRemove = new List<SummaryLicenceViewModel>();

            foreach (var summaryLicense in SummaryLicences)
            {
                if (!licenses.Any(l => l.Url == summaryLicense.Url))
                {
                    toRemove.Add(summaryLicense);
                }
            }

            foreach (var licenseToRemove in toRemove)
            {
                SummaryLicences.Remove(licenseToRemove);
            }

            foreach (var license in licenses)
            {
                if (!SummaryLicences.Any(l => l.Url == license.Url))
                {
                    SummaryLicences.Add(new SummaryLicenceViewModel(license));
                }
            }            
        }
    }
}
