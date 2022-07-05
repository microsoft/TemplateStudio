using System.Windows;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Param_RootNamespace.Constants;
using Param_RootNamespace.Contracts.Services;

namespace Param_RootNamespace.ViewModels;

// You can show pages in different ways (update main view, navigate, right pane, new windows or dialog)
// using the NavigationService, RightPaneService and WindowManagerService.
// Read more about MenuBar project type here:
// https://github.com/microsoft/TemplateStudio/blob/main/docs/WPF/projectTypes/menubar.md
public class ShellViewModel : BindableBase
{
    private readonly IRegionManager _regionManager;
    private readonly IRightPaneService _rightPaneService;
    private IRegionNavigationService _navigationService;
    private DelegateCommand _goBackCommand;
    private ICommand _loadedCommand;
    private ICommand _unloadedCommand;
    private ICommand _menuFileExitCommand;

    public DelegateCommand GoBackCommand => _goBackCommand ?? (_goBackCommand = new DelegateCommand(OnGoBack, CanGoBack));

    public ICommand LoadedCommand => _loadedCommand ?? (_loadedCommand = new DelegateCommand(OnLoaded));

    public ICommand UnloadedCommand => _unloadedCommand ?? (_unloadedCommand = new DelegateCommand(OnUnloaded));

    public ICommand MenuFileExitCommand => _menuFileExitCommand ?? (_menuFileExitCommand = new DelegateCommand(OnMenuFileExit));

    public ShellViewModel(IRegionManager regionManager, IRightPaneService rightPaneService)
    {
        _regionManager = regionManager;
        _rightPaneService = rightPaneService;
    }

    private void OnLoaded()
    {
        _navigationService = _regionManager.Regions[Regions.Main].NavigationService;
        _navigationService.Navigated += OnNavigated;
    }

    private void OnUnloaded()
    {
        _navigationService.Navigated -= OnNavigated;
        _regionManager.Regions.Remove(Regions.Main);
        _rightPaneService.CleanUp();
    }

    private bool CanGoBack()
        => _navigationService != null && _navigationService.Journal.CanGoBack;

    private void OnGoBack()
        => _navigationService.Journal.GoBack();

    private bool RequestNavigate(string target)
    {
        if (_navigationService.CanNavigate(target))
        {
            _navigationService.RequestNavigate(target);
            return true;
        }

        return false;
    }

    private void RequestNavigateAndCleanJournal(string target)
    {
        var navigated = RequestNavigate(target);
        if (navigated)
        {
            _navigationService.Journal.Clear();
        }
    }

    private void OnNavigated(object sender, RegionNavigationEventArgs e)
        => GoBackCommand.RaiseCanExecuteChanged();

    private void OnMenuFileExit()
        => Application.Current.Shutdown();
}
