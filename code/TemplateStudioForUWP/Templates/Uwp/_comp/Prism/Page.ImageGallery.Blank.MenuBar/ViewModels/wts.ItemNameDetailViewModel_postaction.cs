//{[{
using Prism.Commands;
using System.Windows.Input;
//}]}

namespace Param_RootNamespace.ViewModels
{
    public class wts.ItemNameDetailViewModel : ViewModelBase
    {
        private object _selectedImage;
//{[{
        private ICommand _goBackCommand;
//}]}
        public ObservableCollection<SampleImage> Source { get; } = new ObservableCollection<SampleImage>();
//{[{

        public ICommand GoBackCommand => _goBackCommand ?? (_goBackCommand = new DelegateCommand(OnGoBack));
//}]}
        public wts.ItemNameDetailViewModel(INavigationService navigationServiceInstance, ISampleDataService sampleDataServiceInstance, IConnectedAnimationService connectedAnimationService)
        {
        }
//{[{

        private void OnGoBack()
        {
            if (_navigationService.CanGoBack())
            {
                _navigationService.GoBack();
            }
        }
//}]}
    }
}

