public class ViewModelLocator
{
    public ViewModelLocator()
    {
        //^^
        RegisterPivotView(navigationService);
    }

    public PivotViewModel PivotViewModel => ServiceLocator.Current.GetInstance<PivotViewModel>();
    //{[{
    public void RegisterPivotView(NavigationService navigationService)
    {
        SimpleIoc.Default.Register<PivotViewModel>();
        navigationService.Configure(typeof(PivotViewModel).FullName, typeof(PivotView));
    }
    //}]}
}