using System;
using Windows.Networking.Connectivity;

namespace EasyTablesPoc.Helpers
{
    public class InternetConnection
    {
        private static InternetConnection _internetConnection;
        private bool _isInternetAvailable;

        private InternetConnection()
        {
            NetworkInformation.NetworkStatusChanged += (sender) => CheckInternetConnection();
            CheckInternetConnection();
        }

        public static InternetConnection Instance => _internetConnection ?? (_internetConnection = new InternetConnection());

        public event Action<bool> OnInternetAvailabilityChange;

        public bool IsInternetAvailable
        {
            get => _isInternetAvailable;
            protected set
            {
                if (_isInternetAvailable != value)
                {
                    _isInternetAvailable = value;
                    OnInternetAvailabilityChange?.Invoke(value);
                }
            }
        }

        private void CheckInternetConnection()
        {
            try
            {
                var profile = NetworkInformation.GetInternetConnectionProfile();
                IsInternetAvailable = profile?.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }        
    }
}
