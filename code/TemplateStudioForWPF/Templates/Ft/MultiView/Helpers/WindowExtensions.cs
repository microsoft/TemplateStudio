using System.Windows.Controls;

namespace System.Windows;

public static class WindowExtensions
{
    public static object GetDataContext(this Window window)
    {
        if (window.Content is Frame frame)
        {
            return frame.GetDataContext();
        }

        return null;
    }
}
