//{[{
using System.Runtime.CompilerServices;
using System.ComponentModel;
//}]}
namespace Param_ItemNamespace.Views
{
    public sealed partial class wts.ItemNameDetailPage : Page, INotifyPropertyChanged
    {
        //^^

        //{[{
        public event PropertyChangedEventHandler PropertyChanged;

        private void Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            OnPropertyChanged(propertyName);
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}]}
    }
}
