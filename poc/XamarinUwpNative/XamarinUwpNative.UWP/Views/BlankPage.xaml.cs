using System;

using Windows.UI.Xaml.Controls;

using XamarinUwpNative.UWP.ViewModels;

namespace XamarinUwpNative.UWP.Views
{
    public sealed partial class BlankPage : Page
    {
        public BlankViewModel ViewModel { get; } = new BlankViewModel();

        public BlankPage()
        {
            InitializeComponent();
        }
    }
}
