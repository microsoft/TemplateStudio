namespace Param_ItemNamespace.ViewModels
{
    public class PivotViewModel : Conductor<Screen>.Collection.OneActive
    {
        //{[{
        private readonly INavigationService _navigationService;

        public PivotViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        //}]}

        protected override void OnInitialize()
        {
            //^^
            //{[{
            Items.Add(new wts.ItemNameViewModel(_navigationService) { DisplayName = "PivotItem_wts.ItemName/Header".GetLocalized() });
            //}]}
        }
    }
}
