using System;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace AdvancedNavigationPaneProject.Services
{
    public class PageStackEntryEx
    {
        public readonly string FrameKey;

        public readonly NavigationTransitionInfo NavigationTransitionInfo;

        public readonly object Parameter;

        public readonly Type SourcePageType;

        public PageStackEntryEx(string frameKey, NavigationEventArgs args)
        {
            FrameKey = frameKey;
            NavigationTransitionInfo = args.NavigationTransitionInfo;
            Parameter = args.Parameter;
            SourcePageType = args.SourcePageType;
        }

        public override string ToString()
        {
            return $"{FrameKey}: {SourcePageType.Name}";
        }
    }
}
