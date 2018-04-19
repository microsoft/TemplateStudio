using System;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace AdvancedNavigationPaneProject.Services
{
    public class PageStackEntryEx
    {
        public readonly NavigationFrame NavigationFrame;

        public readonly NavigationTransitionInfo NavigationTransitionInfo;

        public readonly object Parameter;

        public readonly Type SourcePageType;

        public PageStackEntryEx(NavigationFrame navigationFrame, NavigationEventArgs args)
        {
            NavigationFrame = navigationFrame;
            NavigationTransitionInfo = args.NavigationTransitionInfo;
            Parameter = args.Parameter;
            SourcePageType = args.SourcePageType;
        }

        public override string ToString()
        {
            return $"{NavigationFrame}: {SourcePageType.Name}";
        }
    }
}
