using MarkdownViewer.ViewModels;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System;
using Windows.System;

namespace MarkdownViewer.Views
{
    public sealed partial class MarkdownPage : Page
    {
        public MarkdownViewModel ViewModel { get; } = new MarkdownViewModel();
        public MarkdownPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Parameter?.ToString()))
            {
                var text = await FileIO.ReadTextAsync(e.Parameter as StorageFile);
                ViewModel.Text = text;

            }
        }

        private async void UiMarkdownText_LinkClicked(object sender, Microsoft.Toolkit.Uwp.UI.Controls.LinkClickedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri(e.Link));
        }
    }
}
