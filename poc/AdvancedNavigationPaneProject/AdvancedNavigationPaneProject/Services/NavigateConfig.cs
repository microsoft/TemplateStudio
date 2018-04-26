using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Animation;

namespace AdvancedNavigationPaneProject.Services
{
    public class NavigateConfig
    {
        public readonly string FrameKey;

        public readonly object Parameter;

        public readonly NavigationTransitionInfo InfoOverride;

        public NavigateConfig(string frameKey)
        {
            FrameKey = frameKey;
        }

        public NavigateConfig(string frameKey, object parameter)
        {
            FrameKey = frameKey;
            Parameter = parameter;
        }

        public NavigateConfig(string frameKey, object parameter, NavigationTransitionInfo infoOverride)
        {
            FrameKey = frameKey;
            Parameter = parameter;
            InfoOverride = infoOverride;
        }

        public static NavigateConfig Default => new NavigateConfig(NavigationService.FrameKeyMain);
    }
}
