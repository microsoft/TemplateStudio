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
        public readonly NavigationFrame NavigationFrame;

        public readonly object Parameter;

        public readonly NavigationTransitionInfo InfoOverride;

        public NavigateConfig(NavigationFrame navigationFrame)
        {
            NavigationFrame = navigationFrame;
        }

        public NavigateConfig(NavigationFrame navigationFrame, object parameter)
        {
            NavigationFrame = navigationFrame;
            Parameter = parameter;
        }

        public NavigateConfig(NavigationFrame navigationFrame, object parameter, NavigationTransitionInfo infoOverride)
        {
            NavigationFrame = navigationFrame;
            Parameter = parameter;
            InfoOverride = infoOverride;
        }

        public static NavigateConfig Defaul => new NavigateConfig(NavigationFrame.Main);
    }
}
