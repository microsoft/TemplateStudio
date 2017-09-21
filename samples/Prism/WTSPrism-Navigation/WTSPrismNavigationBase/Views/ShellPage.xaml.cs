using Prism.Windows.Mvvm;
using Windows.UI.Xaml.Controls;
using WTSPrismNavigationBase.ViewModels;

namespace WTSPrismNavigationBase.Views
{
    public sealed partial class ShellPage : SessionStateAwarePage
    {
        private ShellPageViewModel ViewModel => DataContext as ShellPageViewModel;

        public Frame ShellFrame => shellFrame;

        public void SetRootFrame(Frame frame)
        {
            shellFrame.Content = frame;
            ViewModel.Initialize(frame);
        }

        public ShellPage()
        {
            InitializeComponent();
        }
    }
}
