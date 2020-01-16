public ViewModelLocator()
{
    // Services
//{[{
    SimpleIoc.Default.Register<IPersistAndRestoreService, PersistAndRestoreService>();
//}]}
}