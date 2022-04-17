public ts.ItemNameViewModel(ISystemService systemService)
{
//^^
//{[{
    BrowserBackCommand.ObservesProperty(() => Source);
    BrowserForwardCommand.ObservesProperty(() => Source);
//}]}
}
