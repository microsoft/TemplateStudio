using System.Windows.Controls;

namespace Param_RootNamespace.Views;

public partial class ts.ItemNamePage : Page, INotifyPropertyChanged
{
    private string _text;

    public string Text
    {
        get { return _text; }
        set { Set(ref _text, value); }
    }

    public ts.ItemNamePage()
    {
        InitializeComponent();
    }
}
