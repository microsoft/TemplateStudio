namespace Param_RootNamespace.Views
{
    public sealed partial class wts.ItemNamePage : Page, INotifyPropertyChanged
    {
//^^
//{[{
        private bool _isGoBackButtonVisible;
//}]}
        private SampleOrder _selected;
//^^
//{[{
        public bool IsGoBackButtonVisible
        {
            get { return _isGoBackButtonVisible; }
            set { Set(ref _isGoBackButtonVisible, value); }
        }
//}]}

        public ObservableCollection<SampleOrder> SampleItems { get; private set; } = new ObservableCollection<SampleOrder>();

        public wts.ItemNamePage()
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

        private void OnItemClick(object sender, ItemClickEventArgs e)
        {
            if (twoPaneView.Mode == WinUI.TwoPaneViewMode.SinglePane)
            {
//^^
//{[{
                RefreshIsGoBackButtonVisible();
//}]}
            }
        }

        private void OnModeChanged(WinUI.TwoPaneView sender, object args)
        {
//^^
//{[{

            RefreshIsGoBackButtonVisible();
//}]}
        }

//^^
//{[{
        private void OnGoBack(object sender, ItemClickEventArgs e)
            => TryCloseDetail();

        private void RefreshIsGoBackButtonVisible()
            => IsGoBackButtonVisible = twoPaneView.Mode == WinUI.TwoPaneViewMode.SinglePane && TwoPanePriority == WinUI.TwoPaneViewPriority.Pane2;
//}]}
    }
}