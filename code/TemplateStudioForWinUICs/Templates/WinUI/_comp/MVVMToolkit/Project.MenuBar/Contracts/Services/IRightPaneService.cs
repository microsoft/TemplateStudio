using System;
using Microsoft.UI.Xaml.Controls;

namespace Param_RootNamespace.Contracts.Services
{
    public interface IRightPaneService
    {
        void OpenInRightPane(string pageKey, object parameter = null);

        void Initialize(Frame rightPaneFrame, SplitView splitView);

        void CleanUp();
    }
}
