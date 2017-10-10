using System;
using Windows.UI.Xaml.Controls;
using Prism.Windows.Mvvm;
using wts.ItemName.ViewModels;

namespace wts.ItemName.Views
{
    public sealed partial class ShellPage : Page
    {
        private ShellViewModel ViewModel => DataContext as ShellViewModel;

        public Frame ShellFrame => shellFrame;

        public ShellPage()
        {
            InitializeComponent();
        }

        public void SetRootFrame(Frame frame)
        {
            shellFrame.Content = frame;
            ViewModel.Initialize(frame);
        }
    }
}
