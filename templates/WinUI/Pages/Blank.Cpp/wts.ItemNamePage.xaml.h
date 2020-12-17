#pragma once

#include "wts.ItemNamePage.g.h"

namespace winrt::Param_RootNamespace::implementation
{
    struct wts.ItemNamePage : wts.ItemNamePageT<wts.ItemNamePage>
    {
        wts.ItemNamePage();

        int32_t MyProperty();
        void MyProperty(int32_t value);

        void myButton_Click(Windows::Foundation::IInspectable const& sender, Microsoft::UI::Xaml::RoutedEventArgs const& args);
    };
}

namespace winrt::Param_RootNamespace::factory_implementation
{
    struct wts.ItemNamePage : wts.ItemNamePageT<wts.ItemNamePage, implementation::wts.ItemNamePage>
    {
    };
}
