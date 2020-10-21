//{[{
using MahApps.Metro.Controls;
//}]}
namespace Param_RootNamespace.Contracts.Views
{
    public interface IShellWindow
    {
//^^
//{[{

        Frame GetRightPaneFrame();

        SplitView GetSplitView();
//}]}
    }
}