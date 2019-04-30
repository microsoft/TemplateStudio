namespace Param_RootNamespace
{
    public sealed partial class App : PrismUnityApplication
    {
        public App()
        {
        }
//{[{

        public Type GetPage(string pageToken) => GetPageType(pageToken);
//}]}
    }
}
