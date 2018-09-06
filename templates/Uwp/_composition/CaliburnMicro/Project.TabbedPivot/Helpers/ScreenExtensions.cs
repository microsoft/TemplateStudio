using System;
using Caliburn.Micro;

namespace wts.ItemName.Helpers
{
    public static class ScreenExtensions
    {
        public static T GetPage<T>(this Screen screen)
        {
            if (screen.GetView() is T element)
            {
                return element;
            }

            return default(T);
        }

        public static bool IsOfPageType(this Screen screen, Type pageType)
        {
            if (screen.GetView().GetType() == pageType)
            {
                return true;
            }
            return false;
        }
    }
}