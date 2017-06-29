using Microsoft.Templates.UI.ViewModels.NewItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Microsoft.Templates.UI.Views.NewItem
{
    /// <summary>
    /// Interaction logic for ProjectConfigurationWindow.xaml
    /// </summary>
    public partial class ProjectConfigurationWindow : Window
    {
        public ProjectConfigurationViewModel ViewModel { get; }

        public ProjectConfigurationWindow()
        {
            ViewModel = new ProjectConfigurationViewModel(this);
            DataContext = ViewModel;
            Loaded += ProjectConfigurationWindow_Loaded;

            InitializeComponent();
        }

        private void ProjectConfigurationWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.Initialize();
        }
    }
}
