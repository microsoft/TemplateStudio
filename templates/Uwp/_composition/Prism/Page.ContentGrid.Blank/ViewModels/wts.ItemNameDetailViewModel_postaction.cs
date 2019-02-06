//{[{
using Prism.Commands;
using System.Windows.Input;
//}]}

namespace Param_RootNamespace.ViewModels
{
    public class wts.ItemNameDetailViewModel : ViewModelBase
    {
        private readonly ISampleDataService _sampleDataService;
        private readonly IConnectedAnimationService _connectedAnimationService;
//{[{
        private readonly INavigationService _navigationService;
        private ICommand _goBackCommand;
//}]}
        private SampleOrder _item;

        public SampleOrder Item
        {
            get { return _item; }
            set { SetProperty(ref _item, value); }
        }
//{[{

        public ICommand GoBackCommand => _goBackCommand ?? (_goBackCommand = new DelegateCommand(OnGoBack));

        public wts.ItemNameDetailViewModel(ISampleDataService sampleDataServiceInstance, IConnectedAnimationService connectedAnimationService, INavigationService navigationService)
        {
            // TODO WTS: Replace this with your actual data
            _sampleDataService = sampleDataServiceInstance;
            _connectedAnimationService = connectedAnimationService;
            _navigationService = navigationService;
        }

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

