namespace winrt::Param_RootNamespace::implementation
{
//^^
//{[{

    void wts.ItemNamePage::myButton_Click(IInspectable const&, RoutedEventArgs const&)
    {
        myButton().Content(box_value(L"Clicked"));
    }
//}]}
}
