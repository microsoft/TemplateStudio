public class ViewModelLocator
{
//^^
//{[{
    public wts.ItemNameDetailViewModel wts.ItemNameDetailViewModel
        => SimpleIoc.Default.GetInstance<wts.ItemNameDetailViewModel>();
//}]}

    public ViewModelLocator()
    {
//^^
//{[{
        Register<wts.ItemNameDetailViewModel, wts.ItemNameDetailPage>();
//}]}
    }
}
