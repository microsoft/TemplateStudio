namespace Param_RootNamespace.ViewModels
{
    public class wts.ItemNameViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        public bool IsCollapsed
        {
            set
            {
//^^
//{[{
                OnPropertyChanged(nameof(IsCollapsed));
//}]}
            }
        }
    }
}