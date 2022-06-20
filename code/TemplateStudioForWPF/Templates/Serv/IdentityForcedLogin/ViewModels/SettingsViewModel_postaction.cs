//{[{
using Param_RootNamespace.Core.Contracts.Services;
//}]}
namespace Param_RootNamespace.ViewModels;

{
//^^
//{[{
    private readonly IUserDataService _userDataService;
    private readonly IIdentityService _identityService;
//}]}
    private readonly IThemeSelectorService _themeSelectorService;
//^^
//{[{
    private UserViewModel _user;
//}]}
    private ICommand _setThemeCommand;
    private ICommand _privacyStatementCommand;
//{[{
    private ICommand _logOutCommand;
//}]}
    public string VersionDescription
    {
    }
//^^
//{[{
    public UserViewModel User
    {
        get { return _user; }
        set { Param_Setter(ref _user, value); }
    }
//}]}
    public ICommand SetThemeCommand => _setThemeCommand ?? (_setThemeCommand = new System.Windows.Input.ICommand<string>(OnSetTheme));

    public ICommand PrivacyStatementCommand => _privacyStatementCommand ?? (_privacyStatementCommand = new System.Windows.Input.ICommand(OnPrivacyStatement));
//{[{

    public ICommand LogOutCommand => _logOutCommand ?? (_logOutCommand = new System.Windows.Input.ICommand(OnLogOut));
//}]}
    public SettingsViewModel(/*{[{*/IUserDataService userDataService, IIdentityService identityService/*}]}*/)
    {
//^^
//{[{
        _userDataService = userDataService;
        _identityService = identityService;
//}]}
    }

    public void OnNavigatedTo(Param_OnNavigatedToParams)
    {
//^^
//{[{
        _identityService.LoggedOut += OnLoggedOut;
        _userDataService.UserDataUpdated += OnUserDataUpdated;
        User = _userDataService.GetUser();
//}]}
    }

    public void OnNavigatedFrom(Param_OnNavigatedFromParams)
    {
//^^
//{[{
        UnregisterEvents();
//}]}
    }
//{[{

    private void UnregisterEvents()
    {
        _identityService.LoggedOut -= OnLoggedOut;
        _userDataService.UserDataUpdated -= OnUserDataUpdated;
    }
//}]}
    private void OnPrivacyStatement()
        => _systemService.OpenInWebBrowser(_appConfig.PrivacyStatement);
//^^
//{[{

    private async void OnLogOut()
    {
        await _identityService.LogoutAsync();
    }

    private void OnUserDataUpdated(object sender, UserViewModel userData)
    {
        User = userData;
    }

    private void OnLoggedOut(object sender, EventArgs e)
    {
        UnregisterEvents();
    }
//}]}
}
