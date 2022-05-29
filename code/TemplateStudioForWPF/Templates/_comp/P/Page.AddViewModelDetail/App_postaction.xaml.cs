protected override void RegisterTypes(IContainerRegistry containerRegistry)
{
    // Views
    //{[{
    containerRegistry.RegisterForNavigation<ts.ItemNameDetailPage, ts.ItemNameDetailViewModel>(PageKeys.ts.ItemNameDetail);
    //}]}
}
