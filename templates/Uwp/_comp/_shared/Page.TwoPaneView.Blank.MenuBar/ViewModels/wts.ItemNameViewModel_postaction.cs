namespace Param_RootNamespace.ViewModels
{
    public class wts.ItemNameViewModel : System.ComponentModel.INotifyPropertyChanged
    {
//^^
//{[{
        private bool _isGoBackButtonVisible;
        private ICommand _goBackCommand;
//}]}
        private SampleOrder _selected;
//^^
//{[{
        public bool IsGoBackButtonVisible
        {
            get { return _isGoBackButtonVisible; }
            set { Param_Setter(ref _isGoBackButtonVisible, value); }
        }
//}]}

        public ObservableCollection<SampleOrder> SampleItems { get; private set; } = new ObservableCollection<SampleOrder>();
//^^
//{[{
        public ICommand GoBackCommand => _goBackCommand ?? (_goBackCommand = new RelayCommand(OnGoBack));
//}]}

        public wts.ItemNameViewModel()
        {
        }

        public bool TryCloseDetail()
        {
            if (TwoPanePriority == WinUI.TwoPaneViewPriority.Pane2)
            {
//^^
//{[{
                RefreshIsGoBackButtonVisible();
//}]}
                return true;
            }
        }

        private void OnItemClick()
        {
            if (_twoPaneView.Mode == WinUI.TwoPaneViewMode.SinglePane)
            {
//^^
//{[{
                RefreshIsGoBackButtonVisible();
//}]}
            }
        }

        private void OnModeChanged(WinUI.TwoPaneView twoPaneView)
        {
//^^
//{[{

            RefreshIsGoBackButtonVisible();
//}]}
        }

//^^
//{[{
        private void OnGoBack()
            => TryCloseDetail();

        private void RefreshIsGoBackButtonVisible()
            => IsGoBackButtonVisible = _twoPaneView.Mode == WinUI.TwoPaneViewMode.SinglePane && TwoPanePriority == WinUI.TwoPaneViewPriority.Pane2;
//}]}
    }
}