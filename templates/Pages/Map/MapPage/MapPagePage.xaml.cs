using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;

//PostActionAnchor: ADD USING

// The Map Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ItemNamespace.MapPage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MapPagePage : Page
    {
        //PostActionAnchor: DEFINE VIEW MODEL

        public MapPagePage()
        {
            this.InitializeComponent();

            //PostActionAnchor: SET DATACONTEXT"
        }

        private void OnLoaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            MapControl map = sender as MapControl;
            if (map == null)
            {
                throw new ArgumentNullException("Expected type is MapControl");
            }
            //TODO UWPTemplates: Set your map access key. If you don't have it, request at https://www.bingmapsportal.com/
            //map.AccessKey = "";
            
            //PostActionAnchor: GET VM FROM IOC

            ViewModel.SetMap(map);
        }
    }
}
