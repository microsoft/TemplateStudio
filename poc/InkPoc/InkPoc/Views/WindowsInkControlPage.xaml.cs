using System;

using InkPoc.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace InkPoc.Views
{
    public sealed partial class WindowsInkControlPage : Page
    {
        public WindowsInkControlViewModel ViewModel { get; } = new WindowsInkControlViewModel();

        public WindowsInkControlPage()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            inkControl.OnCut += (sender, args) => System.Diagnostics.Debug.WriteLine("OnCut");
            inkControl.OnCopy += (sender, args) => System.Diagnostics.Debug.WriteLine("OnCopy");
            inkControl.OnPaste += (sender, args) => System.Diagnostics.Debug.WriteLine("OnPaste");
            inkControl.OnUndo += (sender, args) => System.Diagnostics.Debug.WriteLine("OnUndo");
            inkControl.OnZoomIn += (sender, args) => System.Diagnostics.Debug.WriteLine($"OnZoomIn: {args}");
            inkControl.OnZoomOut += (sender, args) => System.Diagnostics.Debug.WriteLine($"OnZoomOut: {args}");           
        }

        private async void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            inkControl.Copy();
            //inkControl.Cut();
            //inkControl.Paste();
            //inkControl.Undo();
            //inkControl.Redo();
            //await inkControl.TransformTextAndShapesAsync();
            //await inkControl.ExportAsImageAsync();
        }
    }
}
