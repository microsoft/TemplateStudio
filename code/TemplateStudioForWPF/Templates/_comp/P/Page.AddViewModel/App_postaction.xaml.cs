protected override void RegisterTypes(IContainerRegistry containerRegistry)
{
    // Views
    //{[{
    containerRegistry.RegisterForNavigation<ts.ItemNamePage, ts.ItemNameViewModel>(PageKeys.ts.ItemName);
    //}]}
}
