public ViewModelLocator()
{
//{[{
    SimpleIoc.Default.Register<IWindowManagerService, WindowManagerService>();
    SimpleIoc.Default.Register<IRightPaneService, RightPaneService>();
//}]}
}