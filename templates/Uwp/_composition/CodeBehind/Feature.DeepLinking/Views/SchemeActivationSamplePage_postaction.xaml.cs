//{[{
using System.Collections.ObjectModel;
//}]}
namespace Param_ItemNamespace.Views
{
    public sealed partial class SchemeActivationSamplePage : Page
    {
//{[{
        public ObservableCollection<string> Parameters { get; } = new ObservableCollection<string>();

//}]}
        public SchemeActivationSamplePage()
        {
            InitializeComponent();
        }
//{[{

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
//}]}
    }
}
