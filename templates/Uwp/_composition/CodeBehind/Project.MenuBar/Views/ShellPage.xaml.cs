using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Param_RootNamespace.Helpers;
using Param_RootNamespace.Services;

namespace Param_RootNamespace.Views
{
    // TODO WTS: You can edit the text for the menu in String/en-US/Resources.resw
    // You can show pages in different ways (update main view, navigate, right pane, new windows or dialog) using MenuNavigationHelper class.
    // Read more about MenuBar project type here:
    // https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/projectTypes/menubar.md
    public sealed partial class ShellPage : Page, INotifyPropertyChanged
    {
        public ShellPage()
        {
            InitializeComponent();
            NavigationService.Frame = shellFrame;
            MenuNavigationHelper.Initialize(splitView, rightFrame);
            KeyboardAccelerators.Add(ActivationService.BackKeyboardAccelerator);
            KeyboardAccelerators.Add(ActivationService.AltLeftKeyboardAccelerator);
        }

        private void ShellMenuItemClick_File_Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            OnPropertyChanged(propertyName);
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
