//{[{
using Param_RootNamespace.Core.Contracts.Services;
using Param_RootNamespace.Core.Services;
//}]}
namespace Param_RootNamespace.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            // Core Services
//{[{
            SimpleIoc.Default.Register<IFileService, FileService>();
//}]}
            // Services
//{[{
            SimpleIoc.Default.Register<IPersistAndRestoreService, PersistAndRestoreService>();
//}]}
        }
    }
}