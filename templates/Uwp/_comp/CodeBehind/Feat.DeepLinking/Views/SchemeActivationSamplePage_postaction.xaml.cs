//{[{
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
//}]}
namespace Param_RootNamespace.Views
{
    public sealed partial class SchemeActivationSamplePage /*{[{*/INotifyPropertyChanged/*}]}*/
    {
//{[{
        public ObservableCollection<string> Parameters { get; } = new ObservableCollection<string>();

//}]}
        public SchemeActivationSamplePage()
        {
            InitializeComponent();
        }
//^^
//{[{

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var parameters = e.Parameter as Dictionary<string, string>;
            if (parameters != null)
            {
                Initialize(parameters);
            }
        }

        public void Initialize(Dictionary<string, string> parameters)
        {
            Parameters.Clear();
            foreach (var param in parameters)
            {
                if (param.Key == "ticks" && long.TryParse(param.Value, out long ticks))
                {
                    var dateTime = new DateTime(ticks);
                    Parameters.Add($"{param.Key}: {dateTime.ToString()}");
                }
                else
                {
                    Parameters.Add($"{param.Key}: {param.Value}");
                }
            }
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
//}]}
    }
}
