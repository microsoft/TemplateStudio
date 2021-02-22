#include "pch.h"
#include "wts.ItemNamePage.xaml.h"
//-:cnd:noEmit
#if __has_include("wts.ItemNamePage.g.cpp")
#include "wts.ItemNamePage.g.cpp"
#endif
//+:cnd:noEmit

using namespace winrt;
using namespace Microsoft::UI::Xaml;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace winrt::Param_RootNamespace::implementation
{
    wts.ItemNamePage::wts.ItemNamePage()
    {
        InitializeComponent();
    }

    int32_t wts.ItemNamePage::MyProperty()
    {
        throw hresult_not_implemented();
    }

    void wts.ItemNamePage::MyProperty(int32_t /* value */)
    {
        throw hresult_not_implemented();
    }

    void wts.ItemNamePage::myButton_Click(IInspectable const&, RoutedEventArgs const&)
    {
        myButton().Content(box_value(L"Clicked"));
    }
}
