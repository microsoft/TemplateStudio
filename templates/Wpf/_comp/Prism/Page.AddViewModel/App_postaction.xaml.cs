protected async override void RegisterTypes(IContainerRegistry containerRegistry)
{
    // Views
//{[{
    containerRegistry.RegisterForNavigation<wts.ItemNamePage, wts.ItemNameViewModel>(PageKeys.wts.ItemName);
//}]}
}