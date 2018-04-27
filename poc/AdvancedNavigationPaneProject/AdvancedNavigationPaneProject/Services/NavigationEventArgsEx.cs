using System;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace AdvancedNavigationPaneProject.Services
{
    public class NavigationEventArgsEx
    {
        public readonly string FrameKey;

        public readonly Uri Uri;

        public readonly object Content;

        public readonly NavigationMode NavigationMode;

        public readonly object Parameter;

        public readonly Type SourcePageType;

        public readonly NavigationTransitionInfo NavigationTransitionInfo;

        public NavigationEventArgsEx(string frameKey, NavigationEventArgs args)
        {
            FrameKey = frameKey;
            SourcePageType = args.SourcePageType;
            Parameter = args.Parameter;
            NavigationMode = args.NavigationMode;
            Content = args.Content;
            NavigationTransitionInfo = args.NavigationTransitionInfo;
            Uri = args.Uri;
        }

        public NavigationEventArgsEx(string frameKey, Type sourcePageType, NavigationConfig config, object content)
        {
            FrameKey = frameKey;
            SourcePageType = sourcePageType;
            Parameter = config.Parameter;
            NavigationMode = NavigationMode.New;
            Content = content;
            NavigationTransitionInfo = config.InfoOverride;
        }
    }
}
