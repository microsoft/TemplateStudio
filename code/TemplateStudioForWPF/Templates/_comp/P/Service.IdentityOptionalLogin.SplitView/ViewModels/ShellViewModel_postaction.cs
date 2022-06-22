//{[{
using Param_RootNamespace.Contracts.Services;
using Param_RootNamespace.Helpers;
using Param_RootNamespace.Core.Contracts.Services;
using Param_RootNamespace.Core.Helpers;
//}]}
namespace Param_RootNamespace.ViewModels;

public class ShellViewModel : /*{[{*/IDisposable/*}]}*/
{
    private readonly IRegionManager _regionManager;
//{[{
    private readonly IIdentityService _identityService;
    private readonly IUserDataService _userDataService;
//}]}
    private HamburgerMenuItem _selectedMenuItem;
//^^
//{[{
    private bool _isBusy;
    private bool _isLoggedIn;
    private bool _isAuthorized;
//}]}
    private DelegateCommand _goBackCommand;
//^^
//{[{
    public Func<HamburgerMenuItem, bool> IsPageRestricted { get; } = (menuItem) =>
    {
        var app = App.Current as App;
        var page = app.GetPageType(menuItem.Tag.ToString());
        return Attribute.IsDefined(page.GetType(), typeof(Restricted));
    };

    public bool IsBusy
    {
        get { return _isBusy; }
        set { SetProperty(ref _isBusy, value); }
    }

    public bool IsLoggedIn
    {
        get { return _isLoggedIn; }
        set { SetProperty(ref _isLoggedIn, value); }
    }

    public bool IsAuthorized
    {
        get { return _isAuthorized; }
        set { SetProperty(ref _isAuthorized, value); }
    }

//}]}
    public DelegateCommand GoBackCommand => _goBackCommand ?? (_goBackCommand = new DelegateCommand(OnGoBack, CanGoBack));

    public ShellViewModel(/*{[{*/IIdentityService identityService, IUserDataService userDataService/*}]}*/)
    {
//^^
//{[{
        _identityService = identityService;
        _userDataService = userDataService;
        _identityService.LoggedIn += OnLoggedIn;
        _identityService.LoggedOut += OnLoggedOut;
        _userDataService.UserDataUpdated += OnUserDataUpdated;
//}]}
    }
//{[{

    public void Dispose()
    {
        _userDataService.UserDataUpdated -= OnUserDataUpdated;
    }

    private void OnUserDataUpdated(object sender, UserViewModel user)
    {
        var userMenuItem = OptionMenuItems.OfType<HamburgerMenuImageItem>().FirstOrDefault();
        if (userMenuItem != null)
        {
            userMenuItem.Label = user.Name;
            userMenuItem.Thumbnail = user.Photo;
        }
    }

    private void OnLoggedIn(object sender, EventArgs e)
    {
        IsLoggedIn = true;
        IsAuthorized = IsLoggedIn && _identityService.IsAuthorized();
        IsBusy = false;
    }

    private void OnLoggedOut(object sender, EventArgs e)
    {
        IsLoggedIn = false;
        IsAuthorized = false;
        RemoveUserInformation();
        _navigationService.Journal.Clear();
    }

    private void RemoveUserInformation()
    {
        var userMenuItem = OptionMenuItems.OfType<HamburgerMenuImageItem>().FirstOrDefault();
        if (userMenuItem != null)
        {
            userMenuItem.Thumbnail = ImageHelper.ImageFromAssetsFile("DefaultIcon.png");
            userMenuItem.Label = Resources.Shell_LogIn;
        }
    }
//}]}
    private void OnLoaded()
    {
//^^
//{[{
        IsLoggedIn = _identityService.IsLoggedIn();
        IsAuthorized = IsLoggedIn && _identityService.IsAuthorized();
        var userMenuItem = new HamburgerMenuImageItem()
        {
            Command = new DelegateCommand(OnUserItemSelected, () => !IsBusy)
        };
        if (IsAuthorized)
        {
            var user = _userDataService.GetUser();
            userMenuItem.Thumbnail = user.Photo;
            userMenuItem.Label = user.Name;
        }
        else
        {
            userMenuItem.Thumbnail = ImageHelper.ImageFromAssetsFile("DefaultIcon.png");
            userMenuItem.Label = Resources.Shell_LogIn;
        }

        OptionMenuItems.Insert(0, userMenuItem);
//}]}
    }
//{[{

    private async void OnUserItemSelected()
    {
        if (!IsLoggedIn)
        {
            IsBusy = true;
            var loginResult = await _identityService.LoginAsync();
            if (loginResult != LoginResultType.Success)
            {
                await AuthenticationHelper.ShowLoginErrorAsync(loginResult);
                IsBusy = false;
            }
        }

        RequestNavigate(PageKeys.Settings);
    }
//}]}
}
