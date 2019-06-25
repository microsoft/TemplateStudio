namespace Param_RootNamespace.ViewModels
{
    public class TreeViewViewModel : System.ComponentModel.INotifyPropertyChanged
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