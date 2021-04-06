using System;
using System.Windows.Controls;

namespace Param_RootNamespace.Contracts.Services
{
    public interface INavigationService
    {
        event EventHandler<string> Navigated;

        bool CanGoBack { get; }

        void Initialize(Frame shellFrame);

        bool NavigateTo(string pageKey, object parameter = null, bool clearNavigation = false);

        void GoBack();

        void UnsubscribeNavigation();

        void CleanNavigation();
    }
}
