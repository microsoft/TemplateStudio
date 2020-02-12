namespace Param_RootNamespace
{
    public partial class App : PrismApplication
    {
        public App()
        {
        }
//{[{

        public object GetPageType(string pageKey)
            => Container.Resolve<object>(pageKey);
//}]}
    }
}