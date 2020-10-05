using System;
using System.Windows.Controls;

namespace Param_RootNamespace.Contracts.Services
{
    public interface INavigationService
    {
        event EventHandler<Type> Navigated;

        bool CanGoBack { get; }

        void Initialize(Frame shellFrame);

        bool NavigateTo(Type pageType, object parameter = null, bool clearNavigation = false);

        void GoBack();

        void UnsubscribeNavigation();

        void CleanNavigation();
    }
}
