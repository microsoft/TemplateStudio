namespace Param_RootNamespace.Services
{
    public class ApplicationHostService : IApplicationHostService
    {
        public ApplicationHostService(/*{[{*/IRightPaneService rightPaneService/*}]}*/)
        {
//^^
//{[{
            rightPaneService.Initialize(_shellWindow.GetRightPaneFrame(), _shellWindow.GetSplitView());
//}]}
        }
    }
}