public class ViewModelLocator
{
    public ViewModelLocator()
    {
        //^^
        Registeruct.ItemName();
    }

    public uct.ItemNameViewModel uct.ItemNameViewModel => ServiceLocator.Current.GetInstance<uct.ItemNameViewModel>();
    //{[{
    public void Registeruct.ItemName()
    {
        SimpleIoc.Default.Register<uct.ItemNameViewModel>();
    }
    //}]}
}