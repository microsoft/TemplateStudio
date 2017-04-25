using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.UI.ViewModels;
using System.Windows;

namespace Microsoft.Templates.UI.Views
{
    /// <summary>
    /// Interaction logic for InformationWindow.xaml
    /// </summary>
    public partial class InformationWindow : Window
    {
        public InformationViewModel ViewModel { get; }

        public InformationWindow(TemplateInfoViewModel template, Window mainWindow)
        {
            ViewModel = new InformationViewModel(this);
            DataContext = ViewModel;

            Loaded += async (sender, e) =>
            {
                await ViewModel.IniatializeAsync(template);
                CenterWindow(mainWindow);
            };

            Unloaded += (sender, e) =>
            {
                ViewModel.UnsuscribeEventHandlers();
            };

            InitializeComponent();
        }        

        public InformationWindow(MetadataInfoViewModel  metadataInfo, Window mainWindow)
        {
            ViewModel = new InformationViewModel(this);
            DataContext = ViewModel;

            Loaded += async (sender, e) =>
            {
                await ViewModel.IniatializeAsync(metadataInfo);
                CenterWindow(mainWindow);
                ViewModel.InformationVisibility = Visibility.Visible;
            };

            Unloaded += (sender, e) =>
            {
                ViewModel.UnsuscribeEventHandlers();
            };

            InitializeComponent();
        }

        private void CenterWindow(Window mainWindow)
        {            
            if (mainWindow.Width < 1200)
            {
                Width = mainWindow.Width * 0.8;
            }
            else
            {
                Width = mainWindow.Width * 0.6;
            }
            if (mainWindow.Height < 700)
            {
                Height = mainWindow.Height * 0.8;
            }
            else
            {
                Height = mainWindow.Height * 0.6;
            }
            this.Left = mainWindow.Left + (mainWindow.Width - this.ActualWidth) / 2;
            this.Top = mainWindow.Top + (mainWindow.Height - this.ActualHeight) / 2;
        }
    }
}
