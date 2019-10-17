using System.Windows.Controls;
using Param_RootNamespace.Contracts.Views;
using MahApps.Metro.Controls;

namespace Param_RootNamespace.Views
{
    public partial class ShellPage : MetroWindow, IShellPage
    {
        public ShellPage()
        {
            InitializeComponent();
        }

        public Frame GetNavigationFrame()
            => shellFrame;

        public Frame GetRightPaneFrame()
            => rightPaneFrame;

        public void ShowWindow()
            => Show();

        public SplitView GetSplitView()
            => splitView;
    }
}
