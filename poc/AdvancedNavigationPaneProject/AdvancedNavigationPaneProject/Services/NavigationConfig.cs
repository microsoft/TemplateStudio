using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Animation;

namespace AdvancedNavigationPaneProject.Services
{
    public class NavigationConfig
    {
        public readonly bool RegisterOnBackStack;

        public readonly object Parameter;

        public readonly NavigationTransitionInfo InfoOverride;

        public NavigationConfig()
        {
            RegisterOnBackStack = true;
        }

        public NavigationConfig(bool registerOnBackStack)
        {
            RegisterOnBackStack = registerOnBackStack;
        }

        public NavigationConfig(object parameter)
        {
            Parameter = parameter;
        }

        public NavigationConfig(bool registerOnBackStack, object parameter)
        {
            RegisterOnBackStack = registerOnBackStack;
            Parameter = parameter;
        }

        public NavigationConfig(bool registerOnBackStack, object parameter, NavigationTransitionInfo infoOverride)
        {
            RegisterOnBackStack = registerOnBackStack;
            Parameter = parameter;
            InfoOverride = infoOverride;            
        }

        public static NavigationConfig Default => new NavigationConfig();
    }
}
