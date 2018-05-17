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

        public NavigationConfig(bool registerOnBackStack = true, object parameter = null, NavigationTransitionInfo infoOverride = null)
        {
            RegisterOnBackStack = registerOnBackStack;
            Parameter = parameter;
            InfoOverride = infoOverride;            
        }

        public static NavigationConfig Default => new NavigationConfig();
    }
}
