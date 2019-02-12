namespace Param_RootNamespace.Views
{
    public sealed partial class ShellPage : Page
    {
        public void SetRootFrame(Frame frame)
        {
            shellFrame.Content = frame;
//{[{
            navigationViewHeaderBehavior.Initialize(frame);
//}]}
            ViewModel.Initialize(frame, navigationView);
        }
    }
}
