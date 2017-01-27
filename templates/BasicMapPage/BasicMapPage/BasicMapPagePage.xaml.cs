using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Page_NS.BasicMapPage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BasicMapPagePage : Page
    {
        public BasicMapPagePage()
        {
            this.InitializeComponent();
            ViewModel = new BasicMapPageViewModel();
            DataContext = ViewModel;
        }
        public BasicMapPageViewModel ViewModel { get; private set; }

        private void OnLoaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            MapControl map = sender as MapControl;
            if (map == null)
            {
                throw new ArgumentNullException("Expected type is MapControl");//TODO: Set your map access key. If you don't have it, request at https://www.bingmapsportal.com/
            }
            map.AccessKey = "";
            ViewModel.SetMap(map);
        }
    }
}
