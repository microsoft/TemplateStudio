namespace System.Windows.Controls;

public static class FrameExtensions
{
    public static void CleanNavigation(this Frame frame)
    {
        while (frame.CanGoBack)
        {
            frame.RemoveBackEntry();
        }
    }
}
