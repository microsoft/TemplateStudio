//{[{
using GalaSoft.MvvmLight.Command;
//}]}
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
                RaisePropertyChanged(nameof(IsCollapsed));
//}]}
            }
        }
    }
}