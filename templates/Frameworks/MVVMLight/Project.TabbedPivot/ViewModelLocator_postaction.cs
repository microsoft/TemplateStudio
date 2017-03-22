public class ViewModelLocator
{
    public ViewModelLocator()
    {
        //^^
        RegisterPivotView();
    }

    public PivotViewModel PivotViewModel => ServiceLocator.Current.GetInstance<PivotViewModel>();
    //{[{
    public void RegisterPivotView()
    {
        SimpleIoc.Default.Register<PivotViewModel>();
    }
    //}]}
}