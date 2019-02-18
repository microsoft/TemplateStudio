//{**
// This code block adds the method `GetContentGridData()` to the SampleDataService of your project.
//**}
//{[{
using System.Threading.Tasks;
//}]}
namespace Param_RootNamespace.Core.Services
{
    public interface ISampleDataService
    {
//^^
//{[{

        ObservableCollection<SampleOrder> GetContentGridData();
//}]}
    }
}
