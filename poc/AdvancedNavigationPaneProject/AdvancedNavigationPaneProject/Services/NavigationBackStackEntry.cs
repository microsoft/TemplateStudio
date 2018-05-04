using System;
using Windows.UI.Xaml.Media.Animation;

namespace AdvancedNavigationPaneProject.Services
{
    public class NavigationBackStackEntry
    {
        public readonly string FrameKey;        

        public readonly object Parameter;

        public readonly Type SourcePageType;

        public readonly NavigationTransitionInfo NavigationTransitionInfo;

        public NavigationBackStackEntry(string frameKey, Type sourcePageType, NavigationConfig config)
        {
            FrameKey = frameKey;
            SourcePageType = sourcePageType;
            Parameter = config.Parameter;
            NavigationTransitionInfo = config.InfoOverride;
        }
    }
}
