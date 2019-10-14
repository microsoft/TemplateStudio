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

        public void ShowWindow()
            => Show();
    }
}
