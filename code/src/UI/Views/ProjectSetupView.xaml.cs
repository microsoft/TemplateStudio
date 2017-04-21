using Microsoft.Templates.UI.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Microsoft.Templates.UI.Views
{
    /// <summary>
    /// Interaction logic for ProjectSetupView.xaml
    /// </summary>
    public partial class ProjectSetupView : Page
    {
        public ProjectSetupViewModel ViewModel { get; }
        public ProjectSetupView()
        {
            ViewModel = MainViewModel.Current.ProjectSetup;
            DataContext = ViewModel;

            Loaded += async (sender, e) =>
            {
                await ViewModel.IniatializeAsync();
            };

            InitializeComponent();
        }
    }
}
