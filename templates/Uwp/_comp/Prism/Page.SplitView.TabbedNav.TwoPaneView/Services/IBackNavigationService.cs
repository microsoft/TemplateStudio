using System;
using Windows.UI.Xaml.Controls;

namespace Param_RootNamespace.Services
{
    public interface IBackNavigationService
    {
        event EventHandler<bool> OnCurrentPageCanGoBackChanged;

        void Initialize(Frame frame);
    }
}
