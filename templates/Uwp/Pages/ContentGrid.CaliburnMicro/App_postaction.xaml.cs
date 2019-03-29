//{[{
using Param_RootNamespace.ViewModels;
//}]}

namespace Param_RootNamespace
{
    public sealed partial class App
    {
        protected override void Configure()
        {
            //^^
            //{[{       
            _container.PerRequest<ContentGridViewDetailViewModel>();
            //}]}
        }
    }
}
