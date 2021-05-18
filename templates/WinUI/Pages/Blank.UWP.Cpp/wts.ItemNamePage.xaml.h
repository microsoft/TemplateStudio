#pragma once

#include "wts.ItemNamePage.g.h"

namespace winrt::Param_RootNamespace::implementation
{
    struct wts.ItemNamePage : wts.ItemNamePageT<wts.ItemNamePage>
    {
        wts.ItemNamePage();

        int32_t MyProperty();
        void MyProperty(int32_t value);
    };
}

namespace winrt::Param_RootNamespace::factory_implementation
{
    struct wts.ItemNamePage : wts.ItemNamePageT<wts.ItemNamePage, implementation::wts.ItemNamePage>
    {
    };
}
