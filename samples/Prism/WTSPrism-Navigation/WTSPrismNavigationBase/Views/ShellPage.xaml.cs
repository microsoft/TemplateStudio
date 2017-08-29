using Prism.Windows.Mvvm;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using WTSPrismNavigationBase.Helpers;
using WTSPrismNavigationBase.Services;
using WTSPrismNavigationBase.ViewModels;

namespace WTSPrismNavigationBase.Views
{
    public sealed partial class ShellPage : SessionStateAwarePage
    {
        private ShellPageViewModel ViewModel
        {
            get { return DataContext as ShellPageViewModel; }
        }

        public Frame ShellFrame
        {        
            get
            {
                return shellFrame;
            }
        }

        public void SetRootFrame(Frame frame)
        {
            shellFrame.Content = frame;
        }

        public ShellPage()
        {            
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            ViewModel.Initialize(shellFrame);
        }
        
    }
}
