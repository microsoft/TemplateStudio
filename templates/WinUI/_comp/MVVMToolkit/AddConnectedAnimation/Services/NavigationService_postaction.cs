//{[{
using Microsoft.Toolkit.Uwp.UI.Animations;
//}]}

namespace Param_RootNamespace.Services
{
    public class NavigationService : INavigationService
    {
//^^
//{[{
        public void SetListDataItemForNextConnectedAnimation(object item)
            => Frame.SetListDataItemForNextConnectedAnimation(item);
//}]}
    }
}
