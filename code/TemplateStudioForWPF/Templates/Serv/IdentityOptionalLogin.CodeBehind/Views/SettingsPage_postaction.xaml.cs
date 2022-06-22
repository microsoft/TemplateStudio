//{[{
using Param_RootNamespace.Helpers;
using Param_RootNamespace.Core.Contracts.Services;
using Param_RootNamespace.Core.Helpers;
//}]}
namespace Param_RootNamespace.Views;

{
//^^
//{[{
    private readonly IUserDataService _userDataService;
    private readonly IIdentityService _identityService;
//}]}
    private readonly IThemeSelectorService _themeSelectorService;
//^^
//{[{
    private bool _isBusy;
    private bool _isLoggedIn;
    private UserData _user;
//}]}
    private string _versionDescription;
//^^
//{[{
    public bool IsBusy
    {
        get => _isBusy;
        set => Set(ref _isBusy, value);
    }

    public bool IsLoggedIn
    {
        get { return _isLoggedIn; }
        set { Set(ref _isLoggedIn, value); }
    }

    public UserData User
    {
        get { return _user; }
        set { Set(ref _user, value); }
    }
//}]}
    public string VersionDescription
    {
    }

    public SettingsPage(/*{[{*/IUserDataService userDataService, IIdentityService identityService/*}]}*/)
    {
//^^
//{[{
        _userDataService = userDataService;
        _identityService = identityService;
//}]}
    }

    public void OnNavigatedTo(object parameter)
    {
//^^
//{[{
        _identityService.LoggedIn += OnLoggedIn;
        _identityService.LoggedOut += OnLoggedOut;
        IsLoggedIn = _identityService.IsLoggedIn();
        _userDataService.UserDataUpdated += OnUserDataUpdated;
        User = _userDataService.GetUser();
//}]}
    }

    public void OnNavigatedFrom()
    {
//^^
//{[{
        UnregisterEvents();
//}]}
    }
//{[{

    private void UnregisterEvents()
    {
        _identityService.LoggedIn -= OnLoggedIn;
        _identityService.LoggedOut -= OnLoggedOut;
        _userDataService.UserDataUpdated -= OnUserDataUpdated;
    }
//}]}
    private void OnPrivacyStatementClick(object sender, RoutedEventArgs e)
        => _systemService.OpenInWebBrowser(_appConfig.PrivacyStatement);
//^^
//{[{
    private async void OnLogIn(object sender, RoutedEventArgs e)
    {
        IsBusy = true;
        var loginResult = await _identityService.LoginAsync();
        if (loginResult != LoginResultType.Success)
        {
            await AuthenticationHelper.ShowLoginErrorAsync(loginResult);
            IsBusy = false;
        }
    }

    private async void OnLogOut(object sender, RoutedEventArgs e)
    {
        await _identityService.LogoutAsync();
    }

    private void OnUserDataUpdated(object sender, UserData userData)
    {
        User = userData;
    }

    private void OnLoggedIn(object sender, EventArgs e)
    {
        IsLoggedIn = true;
        IsBusy = false;
    }

    private void OnLoggedOut(object sender, EventArgs e)
    {
        User = null;
        IsLoggedIn = false;
        IsBusy = false;
    }
//}]}
    public event PropertyChangedEventHandler PropertyChanged;
}
