public ViewModelLocator()
{
    // Services
//{[{
    SimpleIoc.Default.Register<IThemeSelectorService, ThemeSelectorService>();
//}]}
}