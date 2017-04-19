using System.Windows.Controls;

namespace Microsoft.Templates.UI.Services
{
    public static class NavigationService
    {
        private static Frame _frame;

        public static void Initialize(Frame frame, object content)
        {
            _frame = frame;
            _frame.Content = content;
        }

        public static void Navigate(object content)
        {
            _frame.Navigate(content);
        }

        public static void GoBack()
        {
            if (_frame.CanGoBack)
            {
                _frame.GoBack();
            }            
        }        
    }
}
