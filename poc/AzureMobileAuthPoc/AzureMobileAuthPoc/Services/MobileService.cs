using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System.Threading.Tasks;

namespace AzureMobileAuthPoc.Services
{
    public class MobileService
    {
        private const string AzureServiceEndPoint = "http://wtseasytablespoc.azurewebsites.net";
        private const string SqliteDbName = "localstore_3.db";

        private static MobileService _instance;
        public static MobileService Instance => _instance ?? (_instance = new MobileService());

        public MobileServiceClient Client { get; } = new MobileServiceClient(AzureServiceEndPoint);
    }
}
