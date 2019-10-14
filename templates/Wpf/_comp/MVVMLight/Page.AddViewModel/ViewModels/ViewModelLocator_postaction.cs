public class ViewModelLocator
{
//^^
//{[{
    public wts.ItemNameViewModel wts.ItemNameViewModel
        => SimpleIoc.Default.GetInstance<wts.ItemNameViewModel>();
//}]}
    public ViewModelLocator()
    {
//^^
//{[{
        Register<MainViewModel, MainPage>();
//}]}
    }
}
