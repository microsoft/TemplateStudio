using System.Windows;

namespace Microsoft.Templates.VsEmulator.Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainViewModel ViewModel { get; set; }

        public MainView()
        {
            ViewModel = new MainViewModel(this);
            InitializeComponent();

            DataContext = ViewModel;

            Loaded += (o, e) =>
            {
                ViewModel.Initialize();
            };
        }
    }
}
