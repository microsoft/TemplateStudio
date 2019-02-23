namespace Param_RootNamespace.ViewModels
{
    public class wts.ItemNameDetailViewModel : Screen
    {
        public SampleOrder Item
        {
            get { return _item; }
            set { Set(ref _item, value); }
        }

//{[{
        public wts.ItemNameDetailViewModel(IConnectedAnimationService connectedAnimationService)
        {
            _connectedAnimationService = connectedAnimationService;
        }
//}]}
    }
}
