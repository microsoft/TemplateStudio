using System.Windows.Media.Imaging;

namespace Param_RootNamespace.ViewModels;

public class UserViewModel : System.ComponentModel.INotifyPropertyChanged
{
    private string _name;
    private string _userPrincipalName;
    private BitmapImage _photo;

    public string Name
    {
        get => _name;
        set => Param_Setter(ref _name, value);
    }

    public string UserPrincipalName
    {
        get => _userPrincipalName;
        set => Param_Setter(ref _userPrincipalName, value);
    }

    public BitmapImage Photo
    {
        get => _photo;
        set => Param_Setter(ref _photo, value);
    }

    public UserViewModel()
    {
    }
}
