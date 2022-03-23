//{**
// This code block adds the method `GetGridDataAsync()` to the SampleDataService of your project.
//**}
//{[{
using System.Threading.Tasks;
//}]}
    public interface ISampleDataService
    {
//^^
//{[{

        Task<IEnumerable<SampleOrder>> GetGridDataAsync();
//}]}
    }
}
