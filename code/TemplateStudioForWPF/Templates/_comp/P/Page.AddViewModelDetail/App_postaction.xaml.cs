protected override void RegisterTypes(IContainerRegistry containerRegistry)
{
    // Views
//{[{
    containerRegistry.RegisterForNavigation<wts.ItemNameDetailPage, wts.ItemNameDetailViewModel>(PageKeys.wts.ItemNameDetail);
//}]}
}