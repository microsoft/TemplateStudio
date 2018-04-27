using System;

namespace AdvancedNavigationPaneProject.Services
{
    public class NewNavigationEntry
    {
        public readonly string FrameKey;

        public readonly bool RegisterOnBackStack;

        public readonly Type PageType;

        public NewNavigationEntry(Type pageType, string frameKey, NavigationConfig config)
        {
            PageType = pageType;
            FrameKey = frameKey;
            RegisterOnBackStack = config.RegisterOnBackStack;
        }
    }
}
