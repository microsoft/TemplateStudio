//{[{
using MahApps.Metro.Controls;
using System.Windows.Controls;
using Param_RootNamespace.Behaviors;
//}]}
namespace Param_RootNamespace.Contracts.Views
{
    public interface IShellWindow
    {
//^^
//{[{

        Frame GetRightPaneFrame();

        SplitView GetSplitView();

        RibbonTabsBehavior GetRibbonTabsBehavior();
//}]}
    }
}