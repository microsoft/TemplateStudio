//{[{
using MahApps.Metro.Controls;
//}]}
namespace Param_RootNamespace.Contracts.Views
{
    public interface IShellPage
    {
//^^
//{[{

    Frame GetRightPaneFrame();

    SplitView GetSplitView();
//}]}
    }
}