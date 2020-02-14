public wts.ItemNameViewModel(ISystemService systemService)
{
//^^
//{[{
    BrowserBackCommand.ObservesProperty(() => Source);
    BrowserForwardCommand.ObservesProperty(() => Source);
//}]}
}