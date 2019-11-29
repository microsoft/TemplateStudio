using System;
using Windows.UI.Xaml.Controls;
using Param_RootNamespace.Services;
using Param_RootNamespace.ViewModels;

namespace Param_RootNamespace.Views
{
    // TODO WTS: You can edit the text for the menu in String/en-US/Resources.resw
    // You can show pages in different ways (update main view, navigate, right pane, new windows or dialog) using MenuNavigationService class.
    // Read more about MenuBar project type here:
    // https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/UWP/projectTypes/menubar.md
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
            ViewModel.Initialize(frame, splitView, rightFrame);
        }
    }
}
