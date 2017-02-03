using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;

#if (isMVVMLight)
using Microsoft.Practices.ServiceLocation;
#endif

// The Map Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Param_PageNS.MapPage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MapPagePage : Page
    {
        public MapPagePage()
        {
            this.InitializeComponent();
#if (isBasic)
            ViewModel = new MapPageViewModel();
            DataContext = ViewModel;
#endif
        }
#if (isBasic)
        public MapPageViewModel ViewModel { get; private set; }
#endif
        private void OnLoaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            MapControl map = sender as MapControl;
            if (map == null)
            {
                throw new ArgumentNullException("Expected type is MapControl");
            }
            //TODO: UWPTemplates -> Set your map access key. If you don't have it, request at https://www.bingmapsportal.com/
            map.AccessKey = "";
#if (isMVVMLight)
            var ViewModel = ServiceLocator.Current.GetInstance<MapPageViewModel>();
#endif
            ViewModel.SetMap(map);
        }
    }
}
