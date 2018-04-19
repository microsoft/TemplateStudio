using System;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace AdvancedNavigationPaneProject.Services
{
    public class NavigationEventArgsEx
    {
        public readonly NavigationFrame NavigationFrame;

        public readonly Uri Uri;

        public readonly object Content;

        public readonly NavigationMode NavigationMode;

        public readonly object Parameter;

        public readonly Type SourcePageType;

        public readonly NavigationTransitionInfo NavigationTransitionInfo;

        public NavigationEventArgsEx(NavigationFrame navigationFrame, NavigationEventArgs args)
        {
            NavigationFrame = navigationFrame;
            Uri = args.Uri;
            Content = args.Content;
            NavigationMode = args.NavigationMode;
            Parameter = args.Parameter;
            SourcePageType = args.SourcePageType;
            NavigationTransitionInfo = args.NavigationTransitionInfo;
        }
    }
}
