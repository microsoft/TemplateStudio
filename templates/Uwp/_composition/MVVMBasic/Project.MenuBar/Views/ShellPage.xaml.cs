using System;
using Windows.UI.Xaml.Controls;
using Param_RootNamespace.Services;
using Param_RootNamespace.ViewModels;

namespace Param_RootNamespace.Views
{
    // TODO WTS: You can edit the text for the menu in String/en-US/Resources.resw
    public sealed partial class ShellPage : Page
    {
        public ShellViewModel ViewModel { get; } = new ShellViewModel();

        public ShellPage()
        {
            InitializeComponent();
            ViewModel.Initialize(shellFrame, splitView, rightFrame);
            KeyboardAccelerators.Add(ActivationService.BackKeyboardAccelerator);
            KeyboardAccelerators.Add(ActivationService.AltLeftKeyboardAccelerator);
        }
    }
}
